namespace StudentManagerApi.Services
{
    public interface IJwtManager
    {
        Task<string> CreateJwt(string username);
    }
}