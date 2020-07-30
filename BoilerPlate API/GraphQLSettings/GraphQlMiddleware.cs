using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BoilerPlate_API.GraphQLSettings
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphQLSettings _settings;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;

        public GraphQLMiddleware(
            RequestDelegate next,
            GraphQLSettings settings,
            IDocumentExecuter executer,
            IDocumentWriter writer)
        {
            _next = next;
            _settings = settings;
            _executer = executer;
            _writer = writer;
        }

        public async Task Invoke(HttpContext context, ISchema schema)
        {
            if (!IsGraphQLRequest(context))
            {
                await _next(context);
                return;
            }

            await ExecuteAsync(context, schema);
        }

        private bool IsGraphQLRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(_settings.Path)
                && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ExecuteAsync(HttpContext context, ISchema schema)
        {
            var request = Deserialize<GraphQLQuery>(context.Request.Body);
            
            var result = await _executer.ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = request?.Query;
                _.OperationName = request?.OperationName;
                _.Inputs = request?.Variables.ToInputs();
                _.ValidationRules = _settings.ValidationRules;
                _.UserContext = _settings.BuildUserContext?.Invoke(context);
                
            });

            await WriteResponseAsync(context, result);
        }

        
        private async Task WriteResponseAsync(HttpContext context, ExecutionResult result)
        {
            context.Response.ContentType = "application/json";
            if (result.Errors?.Any() == true) {
                if(result.Errors.ElementAt(0).Code.Equals("authorization")) {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                } else if(result.Errors.ElementAt(0).Code.Equals("NOT_FOUND")) {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                } else if(result.Errors.ElementAt(0).Code.Equals("FORBIDEN")) {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                } else if(result.Errors.ElementAt(0).Code.Equals("SERVER_ERROR")) {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                } else if (result.Errors.ElementAt(0).Code.Equals("CONFLICT")){
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                } else {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            } else 
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await _writer.WriteAsync(context.Response.Body, result);
        }

        public static T Deserialize<T>(Stream s)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        } 
    }
}
