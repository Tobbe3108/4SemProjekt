namespace Auth.Application.Common.Models
{
    public class JwtOptions
    {
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
    }
}