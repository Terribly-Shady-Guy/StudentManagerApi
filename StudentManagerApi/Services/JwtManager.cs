using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StudentManagerApi.Models;
using System.Security.Principal;

namespace StudentManagerApi.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly IRsaKeyFileReader _reader;
        private readonly IConfiguration _configuration;

        public JwtManager(IRsaKeyFileReader reader, IConfiguration configuration)
        {
            _reader = reader;
            _configuration = configuration;
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
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public async Task<string> CreateJwtAsync(Claim[] claims)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPrivateKeyFileAsync();

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

        public async Task<Claim[]> ExtractClaims(string expiredToken)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPrivateKeyFileAsync();

            var parameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = rsaPrivateKey,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"]
            };

            var handler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal principal = handler.ValidateToken(expiredToken, parameters, out SecurityToken token);
                var jwtToken = token as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new Claim[1];
                }

                return principal.Claims.ToArray();
            }
            catch (Exception)
            {
                return new Claim[1];
            }
        }
    }
}
