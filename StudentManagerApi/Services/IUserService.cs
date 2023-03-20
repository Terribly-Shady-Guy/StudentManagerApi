using StudentManagerApi.Dtos;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IUserService
    {
        Task<bool> AddNewUserAsync(User user);
        Task<User?> GetUserAsync(LoginDto login);
        Task<bool> UpdateUserRoleAsync(RoleDto user);
    }
}