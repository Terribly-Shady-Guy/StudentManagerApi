using System.Security.Claims;

namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwtAsync(List<Claim> claims);
        Task<List<Claim>> ExtractClaimsAsync(string expiredToken);
    }
}