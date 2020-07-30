using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BoilerPlate_API.Helpers
{
    public class Authentication
    {
        const int _ITERATIONS = 10000;
        const int sizeOfHashingTables = 64;

        private readonly JwtSettings _jwtSettings;
        
        public Authentication(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;

        }

        public static byte[] GenerateRandomSalt(RNGCryptoServiceProvider rng)
        {
            var bytes = new byte[sizeOfHashingTables];
            rng.GetBytes(bytes);
            return bytes;
        }

        public static byte[] HashPassword(string password, byte[] salt)
        {

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, _ITERATIONS))
            {
                return pbkdf2.GetBytes(sizeOfHashingTables);
            }
        }

        public static bool CheckPassword(string password, byte[] salt, byte[] hashedPassword)
        {

            return Enumerable.SequenceEqual(HashPassword(password, salt), hashedPassword);
        }


        //Depending on the implementation this method varies
        /*
        public string GetJwtToken(User user, UserIdentity userIdentity)
        {
            var claims = new List<Claim> {
                new Claim("supreme", userIdentity.CompanyId.ToString()),
                new Claim("userId", userIdentity.UserId.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", userIdentity.Email),
                new Claim("telephone", user.Telephone),
                new Claim(ClaimTypes.Role, user.UserType.Name)
            };

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(_jwtSettings.TokenLifeSpan),
                    issuer: null,
                    audience: null,
                    claims: claims,
                    signingCredentials: SigningCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
         

        }
        */
    }
}
