using StudentManagerApi.Dtos;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public interface IStudentService
    {
        Task<bool> AddNewStudentAsync(StudentDto student);
        Task<List<Student>> GetAllStudentsAsync();
    }
}