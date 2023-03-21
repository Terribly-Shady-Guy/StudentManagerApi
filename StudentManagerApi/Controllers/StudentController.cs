using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            return Ok(await _studentService.GetAllStudentsAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewStudent(StudentDto student)
        {
            bool isSuccessfull = await _studentService.AddNewStudentAsync(student);

            if (isSuccessfull)
            {
                return Ok("new student added");
            }
            else
            {
                return BadRequest("this student already exists");
            }
        }
    }
}
