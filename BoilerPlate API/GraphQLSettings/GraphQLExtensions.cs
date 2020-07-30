using GraphQL.Authorization;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Claims;

namespace BoilerPlate_API.GraphQLSettings
{
    public static class GraphQLExtensions
    {    

        public static void AddGraphQLAuth(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy("AdminPolicy", _ => _.RequireClaim(ClaimTypes.Role, "Admin"));
                authSettings.AddPolicy("UserPolicy", _ => _.RequireClaim(ClaimTypes.Role, "User", "Admin"));
                authSettings.AddPolicy("AuthPolicy", _ => _.RequireAuthenticatedUser());

                return authSettings;
            });
        }


        public static void UseGraphQLWithAuth(this IApplicationBuilder app)
        {
            var settings = new GraphQLSettings
            {
                
                BuildUserContext = ctx =>
                {
                    var principalProvider = app.ApplicationServices.GetService<IHttpContextAccessor>();
                    var principal = principalProvider.HttpContext.User;

                    var userContext = new GraphQLUserContext
                    {
                        User = ctx.User
                    };

                    return userContext;
                }
            };

            var rules = app.ApplicationServices.GetServices<IValidationRule>();
            settings.ValidationRules.AddRange(rules);

            app.UseMiddleware<GraphQLMiddleware>(settings);
        }
    }


}
