namespace StudentManagerApi.Dtos
{
    public class LoggedInDto
    {
        public string AccessToken { get; set; }
        public bool IsAdmin { get; set; }
    }
}
