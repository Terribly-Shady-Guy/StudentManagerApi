using Microsoft.IdentityModel.Tokens;

namespace StudentManagerApi.Services
{
    public interface IRsaKeyFileReader
    {
        Task<RsaSecurityKey> ReadRsaPrivateKeyFileAsync();
        Task<RsaSecurityKey> ReadRsaPublicKeyFileAsync();
    }
}