using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Models;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            throw new NotImplementedException("This endpoint is a work in progress");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewStudent()
        {
            return Ok("Success");
        }
    }
}
