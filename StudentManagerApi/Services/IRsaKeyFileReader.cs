using Microsoft.IdentityModel.Tokens;

namespace StudentManagerApi.Services
{
    public interface IRsaKeyFileReader
    {
        Task<RsaSecurityKey> ReadRsaPrivateKeyFile();
        Task<RsaSecurityKey> ReadRsaPublicKeyFile();
    }
}