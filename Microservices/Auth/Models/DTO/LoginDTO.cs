using System.ComponentModel;

namespace Auth.Api.Models.DTO
{
    public class LoginDTO
    {
        [DefaultValue("Tobbe3108")]
        public string UsernameOrEmail { get; set; }
        [DefaultValue("Zxasqw12")]
        public string Password { get; set; }
    }
}
