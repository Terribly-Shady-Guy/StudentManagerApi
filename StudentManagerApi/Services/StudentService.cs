using Microsoft.EntityFrameworkCore;
using StudentManagerApi.Dtos;
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

        public async Task<bool> AddNewStudentAsync(StudentDto student)
        {
            var newStudent = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                ExpectedGradDate = student.ExpectedGradDate,
                Gpa = student.Gpa,
                Major = student.Major,
            };

            student.Registration.ForEach(r =>
            {
                newStudent.Registrations.Add(new Registration
                {
                    AttendanceType = r.AttendanceType,
                    BookFormat = r.BookFormat,
                    Credits = r.Credits,
                    CourseNumber = r.CourseNumber,
                });
            });

            try
            {
                _context.Students.Add(newStudent);
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
