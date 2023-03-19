using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Services;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtManager _jwtManager;

        public UserController(IJwtManager manager)
        {
            _jwtManager = manager;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(string username)
        {
            return Ok(await _jwtManager.CreateJwt(username));
        }
    }
}
