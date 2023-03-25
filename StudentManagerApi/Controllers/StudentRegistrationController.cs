using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRegistrationController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentRegistrationController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
        {
            List<StudentDto> students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
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
