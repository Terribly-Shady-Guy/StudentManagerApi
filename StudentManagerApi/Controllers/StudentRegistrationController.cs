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
            List<Student> students = await _studentService.GetAllStudentsAsync();

            var studentDtos = new List<StudentDto>();
            foreach (var student in students)
            {
                var studentDto = new StudentDto
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Major = student.Major,
                    ExpectedGradDate = student.ExpectedGradDate,
                    Gpa = student.Gpa,
                };

                foreach (var registration in student.Registrations)
                {
                    studentDto.Registration.Add(new RegistrationDto
                    {
                        AttendanceType = registration.AttendanceType,
                        CourseNumber = registration.CourseNumber,
                        Credits = registration.Credits,
                        BookFormat = registration.BookFormat,
                    });
                }

                studentDtos.Add(studentDto);
            }

            return Ok(studentDtos);
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
