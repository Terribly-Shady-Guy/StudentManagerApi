using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwt(User user);
    }
}