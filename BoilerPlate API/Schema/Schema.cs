using BoilerPlate_API.Mutations;
using BoilerPlate_API.Queries;
using GraphQL.Utilities;
using System;

namespace BoilerPlate_API.Schema
{
    public class Schema : GraphQL.Types.Schema
    {
        public Schema(IServiceProvider services) : base(services)
        {
            Query = services.GetRequiredService<RootQuery>();
            Mutation = services.GetRequiredService<RootMutation>();
        }
    }
}
