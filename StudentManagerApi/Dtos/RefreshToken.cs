namespace StudentManagerApi.Dtos
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
