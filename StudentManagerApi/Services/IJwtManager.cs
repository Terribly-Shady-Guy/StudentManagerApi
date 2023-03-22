using StudentManagerApi.Models;
using System.Security.Claims;

namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwtAsync(User user);
        Task<string> CreateJwtAsync(Claim[] claims);
        Task<Claim[]> ExtractClaims(string expiredToken);
    }
}