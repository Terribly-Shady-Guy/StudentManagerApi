using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtManager _jwtManager;

        public AuthenticationController(IUserService userService, IJwtManager jwtManager)
        {
            _userService = userService;
            _jwtManager = jwtManager;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginDto login)
        {
            User? user = await _userService.GetUserAsync(login);

            if (user is null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            string accessToken = await _jwtManager.CreateJwtAsync(claims);

            SetRefreshToken();

            return Ok(accessToken);
        }

        [HttpPost]
        public async Task<ActionResult<string>> RefreshTokens([FromBody]string accessToken)
        {
            string? refreshToken = Request.Cookies["refresh-token"];

            if (refreshToken is null)
            {
                return Unauthorized();
            }

            List<Claim> claims = await _jwtManager.ExtractClaimsAsync(accessToken);
            if (claims.Count == 0)
            {
                return BadRequest("This is not a valid token");
            }

            string newAccessToken = await _jwtManager.CreateJwtAsync(claims);

            SetRefreshToken();
            return Ok(newAccessToken);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refresh-token");
            return Ok("User signed out");
        }

        private void SetRefreshToken()
        {
            string refreshToken = GenerateRefreshToken();

            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("refresh-token", refreshToken, options);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
