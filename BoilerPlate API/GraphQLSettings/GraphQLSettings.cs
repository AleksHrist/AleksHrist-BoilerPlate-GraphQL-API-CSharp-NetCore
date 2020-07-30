using GraphQL.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace BoilerPlate_API.GraphQLSettings
{
    public class GraphQLSettings
    {
        public Func<HttpContext, IDictionary<string, object>> BuildUserContext { get; set; }

        public PathString Path { get; set; } = "/graphql";

        public List<IValidationRule> ValidationRules { get; } = new List<IValidationRule>();
    }
}