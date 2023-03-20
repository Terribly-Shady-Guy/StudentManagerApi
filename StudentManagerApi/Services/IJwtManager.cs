using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwtAsync(User user);
    }
}