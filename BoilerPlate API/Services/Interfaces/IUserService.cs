using BoilerPlate_API.Database.Models;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlate_API.Services.Interfaces
{
    public interface IUserService
    {
        //Queries
        IEnumerable<User> GetAll();
        Task<User> GetById(long userId, ResolveFieldContext<object> context);

        //Mutations

        Task<User> Add(Dictionary<string, object> user, ResolveFieldContext<object> context);
        Task<User> Delete(long userId, ResolveFieldContext<object> context);
        Task<User> Update(User user, ResolveFieldContext<object> context);
        Task<User> ChangePassword(long userId, string oldPassword, string newPassword, ResolveFieldContext<object> context);

        //Helpers
        Task<int> SaveChanges();
        Task<User> GetByEmail(string email);

    }
}
