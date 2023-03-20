using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Services;
using StudentManagerApi.Models;
using StudentManagerApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagerApi.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public async Task<IActionResult> SignUp(User newUser)
        {
            bool isSuccessfull = await _userService.AddNewUserAsync(newUser);

            if (isSuccessfull)
            {
                return Created(newUser.Username, "New user account created successfully");
            }
            else
            {
                return BadRequest("This user already exists");
            }
        }

        [AllowAnonymous]
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

        [HttpPut]
        public async Task<IActionResult> ModifyUserRole(RoleDto user)
        {
            bool isSuccessfull = await _userService.UpdateUserRoleAsync(user);

            if (isSuccessfull)
            {
                return Ok("User role changed");
            }
            else
            {
                return NotFound("Could not find user");
            }
        }
    }
}
