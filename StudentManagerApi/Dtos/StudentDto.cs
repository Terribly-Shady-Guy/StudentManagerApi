namespace StudentManagerApi.Dtos
{
    public class StudentDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Major { get; set; }

        public DateTime ExpectedGradDate { get; set; }

        public decimal Gpa { get; set; }

        public List<RegistrationDto> Registration { get; set; }

    }
}
