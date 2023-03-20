using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace StudentManagerApi.Services
{
    public class RsaKeyFileReader : IRsaKeyFileReader
    {
        private readonly IConfiguration _configuration;

        public RsaKeyFileReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RsaSecurityKey> ReadRsaPublicKeyFileAsync()
        {
            string publicKey = await ReadFileAsync(_configuration["Rsa:public"]);

            RSA rsaKey = RSA.Create();
            rsaKey.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out int _);
            return new RsaSecurityKey(rsaKey);
        }

        public async Task<RsaSecurityKey> ReadRsaPrivateKeyFileAsync()
        {
            string privateKey = await ReadFileAsync(_configuration["Rsa:private"]);

            RSA rsaKey = RSA.Create();
            rsaKey.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out int _);
            return new RsaSecurityKey(rsaKey);
        }

        private async Task<string> ReadFileAsync(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "..", fileName);
            return await File.ReadAllTextAsync(path);
        }
    }
}
