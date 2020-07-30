using BoilerPlate_API.Database.Models;
using BoilerPlate_API.Services.Interfaces;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlate_API.Services
{
    public class UserService : IUserService
    {
        //private readonly MyDBContext _dbContext;
        public UserService(/*MyDBContext devaGroupContext*/)
        {
            //_dbContext = devaGroupContext;
        }

        //Queries

        public IEnumerable<User> GetAll()
        {
            //return _dbContext.Users.Include(x => x.UserType);
            return null;
        }

        public async Task<User> GetById(long userId, ResolveFieldContext<object> context)
        {
            /*var user = await _dbContext.Users.Include(x => x.UserType)
                                             .FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                GraphQL.ExecutionError gError = new GraphQL.ExecutionError($"User with the id: {userId} doesn't exist");
                gError.Code = "NOT_FOUND";
                context.Errors.Add(gError);
                return null;
            }
            return user;
            */
            return null;
        }


        //Mutations

        public async Task<User> Add(Dictionary<string, object> user, ResolveFieldContext<object> context)
        {
            /* User userToAdd = new User();
            foreach (string _key in user.Keys)
            {
                if (_key.Equals("password"))
                {
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        //Generate the Salt and store it
                        byte[] generatedSalt = Authentication.GenerateRandomSalt(rng);
                        userToAdd.Salt = generatedSalt;

                        //generate password hash
                        userToAdd.Password = Authentication.HashPassword((string)user[_key], generatedSalt);
                    }
                }
                else userToAdd.GetType().GetProperty((_key.First().ToString().ToUpper() + _key.Substring(1))).SetValue(userToAdd, user[_key]);
            }

            
            var userType = await _dbContext.UserTypes.FindAsync(userToAdd.UserTypeId);
            if (userType == null)
            {
                GraphQL.ExecutionError gError = new GraphQL.ExecutionError($"UserType with the id: {userToAdd.UserTypeId} doesn't exist");
                gError.Code = "NOT_FOUND";
                context.Errors.Add(gError);
                return null;
            }

            if ((await GetByEmail(userToAdd.Email)) != null)
            {
                GraphQL.ExecutionError gError = new GraphQL.ExecutionError($@"Email: ""{userToAdd.Email}"" already exists!");
                gError.Code = "CONFLICT";
                context.Errors.Add(gError);
                return null;
            }


            ///////---------------------------------------////////
            ///uncomment for email sending

            // EmailSender.SendEmail(userToAdd.FirstName, userToAdd.Email);

            ///////---------------------------------------////////

            var addUserResult = await _dbContext.AddAsync(userToAdd);
            _ = await SaveChanges();
            return addUserResult.Entity; */
            return null;

        }

        public async Task<User> Delete(long userId, ResolveFieldContext<object> context)
        {
            /*
            var user = await GetById(userId, context);
            if (user == null)
            {
                //already handled
                return null;
            }

            _dbContext.Remove(user);
            _ = await SaveChanges();
            return user; */

            return null;

        }

        public async Task<User> Update(User user, ResolveFieldContext<object> context)
        {
            /*
            var dbUser = await GetById(user.UserId, context);
            if (dbUser == null)
            {
                //Error already handled
                return null;
            }
            if (!dbUser.Email.Equals(user.Email) && (await GetByEmail(user.Email)) != null)
            {
                GraphQL.ExecutionError gError = new GraphQL.ExecutionError($@"Email: ""{user.Email}"" already exists!");
                gError.Code = "CONFLICT";
                context.Errors.Add(gError);
                return null;

            }

            dbUser.Name = user.Name;
            dbUser.Email = user.Email;

            _ = await SaveChanges();
            return dbUser; */
            return null;
        }

        public async Task<User> ChangePassword(long userId, string oldPassword, string newPassword, ResolveFieldContext<object> context)
        {
           /*
            var user = await GetById(userId, context);
            if (user == null)
            {
                //Error already handled
                return null;
            }

            if (!Authentication.CheckPassword(oldPassword, user.Salt, user.Password))
            {
                GraphQL.ExecutionError gError = new GraphQL.ExecutionError($"Wrong password");
                gError.Code = "FORBIDEN";
                context.Errors.Add(gError);
                return null;

            }

            //generate a new SALT(!) and a new hashed password
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                //Generate the Salt and store it
                byte[] generatedSalt = Authentication.GenerateRandomSalt(rng);
                user.Salt = generatedSalt;

                //generate password hash
                user.Password = Authentication.HashPassword(newPassword, generatedSalt);
            }

            _ = await SaveChanges();
            return user;
            */

            return null;

        }

        

        //Helpers
        public async Task<int> SaveChanges()
        {
            //return await _dbContext.SaveChangesAsync();
            return -1;
        }

        public async Task<User> GetByEmail(string email)
        {
            //return await _dbContext.Users.SingleOrDefaultAsync(_user => _user.Email.Equals(email));
            return null;
        }

    }
}
