using StudentManagerApi.Models;

namespace StudentManagerApi.Dtos
{
    public class StudentDto
    {
        public StudentDto()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Major = string.Empty;
            ExpectedGradDate = DateTime.MinValue;
            Gpa = 0;
            Registration = new List<RegistrationDto>();
        }
        public StudentDto(Student student)
        {
            FirstName = student.FirstName;
            LastName = student.LastName;
            Major = student.Major;
            ExpectedGradDate = student.ExpectedGradDate;
            Gpa = student.Gpa;
            Registration = new List<RegistrationDto>();

            foreach (var registration in student.Registrations)
            {
                Registration.Add(new RegistrationDto()
                {
                    AttendanceType = registration.AttendanceType,
                    BookFormat = registration.BookFormat,
                    CourseNumber = registration.CourseNumber,
                    Credits = registration.Credits,
                });
            }
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Major { get; set; }

        public DateTime ExpectedGradDate { get; set; }

        public decimal Gpa { get; set; }

        public List<RegistrationDto> Registration { get; set; }

    }
}
