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
        public async Task<string> CreateJwtAsync(User user)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPrivateKeyFileAsync();

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var credentials = new SigningCredentials(rsaPrivateKey, SecurityAlgorithms.RsaSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = credentials,
                Audience = "student-manager-client",
                Issuer = "student-manager-api"
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
