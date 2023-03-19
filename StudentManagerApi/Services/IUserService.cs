using StudentManagerApi.Dtos;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IUserService
    {
        Task<bool> AddNewUser(User user);
        Task<User?> GetUser(LoginDto login);
        Task<bool> UpdateUserRole(RoleDto user);
    }
}