using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Syncfusion.XForms.Core;

namespace XamarinApp.Data
{
    public class LoginInformation
    {
        [Display(Name = "Username or Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserId should not be empty")]
        [StringLength(200, ErrorMessage = "UserId should not exceed 200 characters")]
        public string UserId { get; set; } = "Tobbe3108";

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password should not be empty")]
        [StringLength(200, ErrorMessage = "Password should not exceed 200 characters")]
        public string Password { get; set; } = "Zxasqw12";
        
    }
}