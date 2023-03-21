using Microsoft.EntityFrameworkCore;
using StudentManagerApi.Models;

namespace StudentManagerApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentManagerDbContext _context;

        public StudentService(StudentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<bool> AddNewStudentAsync(Student student)
        {
            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
