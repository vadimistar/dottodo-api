namespace dottodo.Auth
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
