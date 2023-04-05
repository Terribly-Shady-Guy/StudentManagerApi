using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagerApi.Dtos;
using StudentManagerApi.Models;
using StudentManagerApi.Services;

namespace StudentManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewCourse(Course course)
        {
            bool isSuccessfull = await _courseService.AddNewCourseAsync(course);
            if (isSuccessfull)
            {
                return Created(course.CourseNumber, "New course created");
            }
            else
            {
                return BadRequest("Failed to add new course");
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetCourseNumbers()
        {
            return Ok(await _courseService.GetAllCourseNumbersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetSelectedCourse(string courseNumber)
        {
            Course? course = await _courseService.GetCourseByCourseNumberAsync(courseNumber);

            if (course == null)
            {
                return NotFound();
            }

            var courseDto = new CourseDto
            {
                CourseNumber = course.CourseNumber,
                CourseName = course.CourseName,
                Instructor = course.Instructor,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
            };

            return Ok(courseDto);
        }
    }
}
