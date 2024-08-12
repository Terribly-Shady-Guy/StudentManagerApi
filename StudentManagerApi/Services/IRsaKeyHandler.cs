using Microsoft.IdentityModel.Tokens;

namespace StudentManagerApi.Services
{
    public interface IRsaKeyHandler
    {
        public Task CreateKey();
        public Task<RsaSecurityKey?> GetPublicKey();
        public Task<RsaSecurityKey?> GetPrivateKey();
    }
}
