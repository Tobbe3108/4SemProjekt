using System.ComponentModel.DataAnnotations;

namespace XamarinApp.Data
{
    public class RegisterInformation
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username should not be empty")]
        [StringLength(200, ErrorMessage = "Username should not exceed 200 characters")]
        public string Username { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email should not be empty")]
        [StringLength(200, ErrorMessage = "Email should not exceed 200 characters")]
        public string Email { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName should not be empty")]
        [StringLength(200, ErrorMessage = "FirstName should not exceed 200 characters")]
        public string FirstName { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastName should not be empty")]
        [StringLength(200, ErrorMessage = "LastName should not exceed 200 characters")]
        public string LastName { get; set; }
        
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password should not be empty")]
        [StringLength(200, ErrorMessage = "Password should not exceed 200 characters")]
        public string Password { get; set; }
    }
}