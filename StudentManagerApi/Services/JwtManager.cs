using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;

namespace StudentManagerApi.Services
{
    public class JwtConfig
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }

    public class JwtManager : IJwtManager
    {
        private readonly RsaKeyFileHandler _keyHandler;
        private readonly IOptions<JwtConfig> _config;

        public JwtManager(RsaKeyFileHandler keyHandler, IOptions<JwtConfig> config)
        {
            _keyHandler = keyHandler;
            _config = config;
        }

        public async Task<string> CreateJwtAsync(List<Claim> claims)
        {
            RsaSecurityKey? rsaPrivateKey = await _keyHandler.GetPrivateKey();

            if (rsaPrivateKey is null)
            {
                return string.Empty;
            }

            SigningCredentials credentials = new(rsaPrivateKey, SecurityAlgorithms.RsaSha256);

            var decriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = credentials,
                Audience = _config.Value.Audience,
                Issuer = _config.Value.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(5),
                Subject = new ClaimsIdentity(claims)
            };

            return new JsonWebTokenHandler().CreateToken(decriptor);
        }

        public async Task<List<Claim>> ExtractClaimsAsync(string expiredToken)
        {
            RsaSecurityKey? rsaPublicKey = await _keyHandler.GetPublicKey();

            if (rsaPublicKey is null)
            {
                return new List<Claim>();
            }

            var parameters = new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = rsaPublicKey,
                ValidAudience = _config.Value.Audience,
                ValidIssuer = _config.Value.Issuer,
            };

            var handler = new JsonWebTokenHandler();

            TokenValidationResult result = await handler.ValidateTokenAsync(expiredToken, parameters);

            if (!result.IsValid || result.SecurityToken is not JsonWebToken token)
            {
                return new List<Claim>();
            }

            if (!token.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new List<Claim>();
            }
            
            return result.ClaimsIdentity.Claims.ToList();
        }

        public static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
