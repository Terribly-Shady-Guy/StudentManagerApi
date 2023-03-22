using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;

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

            string token = await _jwtManager.CreateJwtAsync(user);
            return Ok(token);
        }

    }
}
