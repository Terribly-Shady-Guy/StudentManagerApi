using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IRsaKeyFileReader _reader;

        public JwtManager(IRsaKeyFileReader reader)
        {
            _reader = reader;
        }
        public async Task<string> CreateJwt(User user)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPrivateKeyFile();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(rsaPrivateKey, SecurityAlgorithms.RsaSha256),
                Audience = "student-manager-client",
                Issuer = "student-manager-api"
            };
            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
