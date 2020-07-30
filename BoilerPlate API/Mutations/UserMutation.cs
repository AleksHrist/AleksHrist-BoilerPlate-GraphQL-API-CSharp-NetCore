using BoilerPlate_API.Database.Models;
using BoilerPlate_API.Services.Interfaces;
using BoilerPlate_API.Types.User;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlate_API.Mutations
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IUserService userService)
        {
            Field<Types.User.UserType>("addUser",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserCreateType>> { Name = "user" }),
                resolve: context =>
                {
                    Dictionary<string, object> user = context.GetArgument<object>("user") as Dictionary<string, object>;
                    return Task.Run(async () => await userService.Add(user, context)).Result;
                }
                );

            Field<Types.User.UserType>(
               "deleteUser",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "id" }),
               resolve: context => {
                   return Task.Run(async () => await userService.Delete(context.GetArgument<long>("id"), context)).Result;
               }
           );

            Field<Types.User.UserType>(
                "updateUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserCreateType>> { Name = "user" }),
                resolve: context => {
                    return Task.Run(async () => await userService.Update(context.GetArgument<User>("user"), context)).Result;
                }
            );

            Field<Types.User.UserType>("changePassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LongGraphType>> { Name = "userId" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "oldPassword" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "newPassword" }
                    ),
                resolve: context => {
                    return Task.Run(async () => await userService.ChangePassword(
                        context.GetArgument<long>("userId"),
                        context.GetArgument<string>("oldPassword"),
                        context.GetArgument<string>("newPassword"),
                        context)).Result;
                }
            );


        }
    }
}
