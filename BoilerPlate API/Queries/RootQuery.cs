using GraphQL.Types;

namespace BoilerPlate_API.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Name = "RootQuery";
            //Field<UserQuery>("users", resolve: context => new { });
        }
    }
}
