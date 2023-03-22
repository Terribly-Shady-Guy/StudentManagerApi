using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public async Task<string> CreateJwtAsync(List<Claim> claims)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPrivateKeyFileAsync();

            var credentials = new SigningCredentials(rsaPrivateKey, SecurityAlgorithms.RsaSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = credentials,
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        public async Task<List<Claim>> ExtractClaimsAsync(string expiredToken)
        {
            RsaSecurityKey rsaPrivateKey = await _reader.ReadRsaPublicKeyFileAsync();

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
                    return new List<Claim>();
                }

                return principal.Claims.ToList();
            }
            catch (Exception)
            {
                return new List<Claim>();
            }
        }
    }
}
