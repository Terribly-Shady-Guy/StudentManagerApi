using StudentManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagerApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly StudentManagerDbContext _context;

        public CourseService(StudentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetCourseByCourseNumberAsync(string courseNumber)
        {
            return await _context.Courses.FindAsync(courseNumber);
        }

        public async Task<bool> AddNewCourseAsync(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<string>> GetAllCourseNumbersAsync()
        {
            return await _context.Courses.Select(c => c.CourseNumber).ToListAsync();
        }
    }
}
