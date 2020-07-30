using GraphQL.Authorization;
using System.Collections.Generic;
using System.Security.Claims;

namespace BoilerPlate_API.GraphQLSettings
{
    public class GraphQLUserContext : Dictionary<string, object>, IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}