using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.IdentityModel.Tokens;

using Safe_Sign.DTO.User;

namespace Safe_Sign.Util
{
    public static class UserAuthenticationTools
    {
        public static string GenerateToken(UserDTO userDTO)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            string? envKey = Environment.GetEnvironmentVariable("SAFESIGN_KEY");

            if (!string.IsNullOrEmpty(envKey))
            {
                byte[] key = Encoding.ASCII.GetBytes(envKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userDTO.Username.ToString()),
                        new Claim(ClaimTypes.Role, userDTO.IdProfile.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }

            else return "Error to generate a new token";
        }
    }
}
