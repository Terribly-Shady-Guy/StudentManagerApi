using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Services;
using StudentManagerApi.Models;
using StudentManagerApi.Dtos;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;
        private readonly UserService _userService;

        public UserController(IJwtManager manager, UserService userService)
        {
            _jwtManager = manager;
            _userService = userService;
        }

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
