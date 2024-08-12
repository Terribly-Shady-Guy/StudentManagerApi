using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagerApi.Services;

namespace StudentManagerApi
{
    public static class JwtAuthenticationExtensions
    {
        public static void AddRsaJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<RsaKeyFileHandler>();
            services.Configure<RsaKeyConfig>(configuration.GetSection("Rsa"));

            services.AddTransient<IJwtManager, JwtManager>();
            services.Configure<JwtConfig>(configuration.GetSection("Jwt"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var config = Options.Create<RsaKeyConfig>(configuration?.GetSection("Rsa")?.Get<RsaKeyConfig>());

                var handler = new RsaKeyFileHandler(config);
                handler.CreateKey().GetAwaiter().GetResult();

                RsaSecurityKey? rsaPublicKey = handler.GetPublicKey().GetAwaiter().GetResult();

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaPublicKey,
                    ValidAudience = configuration.GetSection("Jwt")
                    .GetValue<string>("Audience"),

                    ValidIssuer = configuration.GetSection("Jwt")
                    .GetValue<string>("Issuer"),

                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                };
            });

        }
    }
}
