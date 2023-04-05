using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface ICourseService
    {
        Task<bool> AddNewCourseAsync(Course course);
        Task<Course?> GetCourseByCourseNumberAsync(string courseNumber);
        Task<List<string>> GetAllCourseNumbersAsync();
    }
}