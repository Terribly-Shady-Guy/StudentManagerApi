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

        public async Task<RsaSecurityKey> ReadRsaPublicKeyFile()
        {
            string publicKey = await ReadFile(_configuration["Rsa:public"]);

            RSA rsaKey = RSA.Create();
            rsaKey.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out int _);
            return new RsaSecurityKey(rsaKey);
        }

        public async Task<RsaSecurityKey> ReadRsaPrivateKeyFile()
        {
            string privateKey = await ReadFile(_configuration["Rsa:private"]);

            RSA rsaKey = RSA.Create();
            rsaKey.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out int _);
            return new RsaSecurityKey(rsaKey);
        }

        private async Task<string> ReadFile(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "..", fileName);
            return await File.ReadAllTextAsync(path);
        }
    }
}
