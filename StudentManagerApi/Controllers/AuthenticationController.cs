using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;
using System.Security.Claims;
using System.Security.Cryptography;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
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

            string accessToken = await _jwtManager.CreateJwtAsync(user);

            SetRefreshToken();

            return Ok(accessToken);
        }

        [HttpGet]
        public async Task<ActionResult<string>> RefreshTokens(string accessToken)
        {
            string? refreshToken = Request.Cookies["refresh-token"];

            if (refreshToken is null)
            {
                return Unauthorized();
            }

            Claim[] claims = await _jwtManager.ExtractClaims(accessToken);
            if (claims.Length == 0)
            {
                return BadRequest("This is not a valid token");
            }

            string newAccessToken = await _jwtManager.CreateJwtAsync(claims);

            SetRefreshToken();
            return Ok(newAccessToken);
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refresh-token");
            return Ok("User signed out");
        }

        private void SetRefreshToken()
        {
            var refreshToken = GenerateRefreshToken();

            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2),
                Secure = true,
            };

            Response.Cookies.Append("refresh-token", refreshToken.ToString(), options);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
