using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Services;
using StudentManagerApi.Models;
using StudentManagerApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;
        private readonly IUserService _userService;

        public UserController(IJwtManager manager, IUserService userService)
        {
            _jwtManager = manager;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SignUp(User newUser)
        {
            bool isSuccessfull = await _userService.AddNewUser(newUser);

            if (isSuccessfull)
            {
                return Created(newUser.Username, "New user account created successfully");
            }
            else
            {
                return BadRequest("This user already exists");
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginDto login)
        {
            User? user = await _userService.GetUser(login);

            if (user is null)
            {
                return Unauthorized();
            }

            string token = await _jwtManager.CreateJwt(user);
            return Ok(token);
        }
    }
}
