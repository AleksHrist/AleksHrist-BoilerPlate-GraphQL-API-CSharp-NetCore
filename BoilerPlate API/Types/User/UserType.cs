using GraphQL.Types;

namespace BoilerPlate_API.Types.User
{
    public class UserType : ObjectGraphType<Database.Models.User>
    {
        public UserType()
        {
            Field(x => x.UserId);
            Field(x => x.Name);
            Field(x => x.Email);
        }
    }
}
