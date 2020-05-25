using System.ComponentModel.DataAnnotations;

namespace XamarinApp.Domain.Entities
{
    public class LoginInformation
    {
        [Display(Name = "Username or Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserId should not be empty")]
        [StringLength(200, ErrorMessage = "UserId should not exceed 200 characters")]
        public string UserId { get; set; } = "Admin";

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password should not be empty")]
        [StringLength(200, ErrorMessage = "Password should not exceed 200 characters")]
        public string Password { get; set; } = "Zxasqw12";
        
    }
}