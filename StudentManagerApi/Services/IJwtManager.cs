using System.Security.Claims;

namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwtAsync(Claim[] claims);
        Task<Claim[]> ExtractClaimsAsync(string expiredToken);
    }
}