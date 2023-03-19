using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;

namespace StudentManagerApi.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;
        private readonly IRsaKeyFileReader _reader;

        public JwtManager(IConfiguration configuration, IRsaKeyFileReader reader)
        {
            _config = configuration;
            _reader = reader;
        }
        public async Task<string> CreateJwt(string username)
        {
            RsaSecurityKey rsa = await _reader.ReadRsaPrivateKeyFile();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(rsa, SecurityAlgorithms.RsaSha256),
            };
            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
