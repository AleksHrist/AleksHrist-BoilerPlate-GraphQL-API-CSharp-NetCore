using BoilerPlate_API.Services.Interfaces;
using BoilerPlate_API.Types.User;
using GraphQL.Types;
using System.Threading.Tasks;

namespace BoilerPlate_API.Queries
{
    public class UserQuery : ObjectGraphType
    {
        public UserQuery(IUserService userService)
    {
        Field<ListGraphType<UserType>>(
         "getAll",
         resolve: context => userService.GetAll()
         );

        Field<UserType>(
            "user",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
            resolve: context => Task.Run(async () => await userService.GetById(context.GetArgument<long>("id"), context)).Result
            );
    }
}
}
