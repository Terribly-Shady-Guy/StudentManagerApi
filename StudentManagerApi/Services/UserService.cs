using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public class UserService : IUserService
    {
        private readonly StudentManagerDbContext _context;

        public UserService(StudentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewUserAsync(User user)
        {
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);

            try
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<User?> GetUserAsync(LoginDto login)
        {
            List<User> users = await _context.Users.Where(u => u.Username == login.Username).ToListAsync();

            if (users.Count == 0)
            {
                return null;
            }

            var hasher = new PasswordHasher<User>();
            foreach (var user in users)
            {
                var result = hasher.VerifyHashedPassword(user, user.Password, login.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<bool> UpdateUserRoleAsync(RoleDto user)
        {
            int rowsChanged = await _context.Users.Where(u => u.Username == user.Username)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.Role, user.Role));

            return rowsChanged > 0;
        }

        public async Task<string?> GetRefreshToken(string name)
        {
            return await _context.Users.Where(u => u.Username == name)
                .Select(u => u.RefreshToken)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SetRefreshToken(string? refreshToken, string userName)
        {
            User? user = await _context.Users.Where(u => u.Username == userName).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }
            
            return await SetRefreshToken(refreshToken, user);
        }

        public async Task<bool> SetRefreshToken(string? refreshToken, User user)
        {
            user.RefreshToken = refreshToken;
            
            try
            {
                _context.Update<User>(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
