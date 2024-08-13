using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;
using System.Security.Claims;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IJwtManager _jwtManager;

        public AuthenticationController(UserService userService, IJwtManager jwtManager)
        {
            _userService = userService;
            _jwtManager = jwtManager;
        }

        [HttpPost]
        public async Task<ActionResult<LoggedInDto>> Login(LoginDto login)
        {
            User? user = await _userService.GetUserAsync(login);

            if (user is null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role),
            };

            string accessToken = await _jwtManager.CreateJwtAsync(claims);

            var loggedInDto = new LoggedInDto
            {
                AccessToken = accessToken,
                IsAdmin = user.Role == "Admin"
            };

            string refreshToken = JwtManager.GenerateRefreshToken();

            await _userService.SetRefreshToken(refreshToken, user);

            SetRefreshTokenCookie(refreshToken);

            return Ok(loggedInDto);
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

            string userName = string.Empty;

            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                    userName = claim.Value;
                    break;
                }
            }

            string? dbRefreshToken = await _userService.GetRefreshToken(userName);

            if (dbRefreshToken is null)
            {
                return Unauthorized();
            }
            else if (refreshToken != dbRefreshToken)
            {
                return Forbid();
            }

            string newAccessToken = await _jwtManager.CreateJwtAsync(claims);

            string newRefreshToken = JwtManager.GenerateRefreshToken();

            bool isSuccesful = await _userService.SetRefreshToken(newRefreshToken, userName);
            if (!isSuccesful)
            {
                return NotFound();
            }

            SetRefreshTokenCookie(newRefreshToken);

            return Ok(newAccessToken);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("refresh-token");
            string? accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken is null)
            {
                return BadRequest();
            }

            List<Claim> claims = await _jwtManager.ExtractClaimsAsync(accessToken);

            string userName = string.Empty;

            foreach (Claim claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {

                    userName = claim.Value;
                    break; 
                }
            }

            await _userService.SetRefreshToken(null, userName);
            return Ok("User signed out");
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
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
    }
}
