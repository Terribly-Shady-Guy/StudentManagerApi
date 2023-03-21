using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IStudentService
    {
        Task<bool> AddNewStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
    }
}