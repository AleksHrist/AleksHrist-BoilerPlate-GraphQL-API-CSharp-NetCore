using BoilerPlate_API.GraphQLSettings;
using BoilerPlate_API.Helpers;
using BoilerPlate_API.Mutations;
using BoilerPlate_API.Queries;
using BoilerPlate_API.Services;
using BoilerPlate_API.Services.Interfaces;
using BoilerPlate_API.Types.User;
using GraphiQl;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var contentRootPath = env.ContentRootPath;
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);


            //Something like AddMVC() ...
            services.AddRazorPages().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            //Json serializing problems
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //Authentication...
            //________________________________________________________________________________________________________________________________

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       RequireSignedTokens = true,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ClockSkew = TimeSpan.Zero,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSettings:Secret"]))
                   };
                   //In case of expiration add a header value for easier checking
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                               context.Response.Headers.Add("Token-Expired", "true");

                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddGraphQLAuth();

            //________________________________________________________________________________________________________________________________

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins(new[] { "https://myapp.netlify.app", "http://myapp.netlify.app"
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));


            //Dependency Injection

            //Services
            services.AddScoped<IUserService, UserService>();

            //DB
            //services.AddDbContext<MyDBContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevaGroupAppDbConnection")));
            
            //GraphQL
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            //Helpers
            services.AddSingleton(jwtSettings);
            services.AddScoped<Authentication>();

            ////Root Query
            services.AddScoped<RootQuery>();

            //Queries
            services.AddScoped<UserQuery>();


            ////Root Mutation
            services.AddScoped<RootMutation>();

            //Mutations
            services.AddScoped<UserMutation>();


            ////Types
            //Query Types
            services.AddScoped<UserType>();


            //Mutation Types
            //User
            services.AddScoped<UserCreateType>();


            //SCHEMA
            //services.AddScoped<ISchema, Schema>();




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphiQl();
            app.UseRouting();
            app.UseCors("CorsPolicy");


            //If you want to access static files from your computer
            /*
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(@"C:\aslike"),
                RequestPath = new PathString("/img")
            }); */

            app.UseAuthentication();
            app.UseGraphQLWithAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
