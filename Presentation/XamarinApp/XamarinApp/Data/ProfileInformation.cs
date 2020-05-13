using System;
using System.ComponentModel.DataAnnotations;

namespace XamarinApp.Data
{
    public class ProfileInformation
    {
        [StringLength(200, ErrorMessage = "Username should not exceed 200 characters")]
        public string Username { get; set; }
        
        [StringLength(200, ErrorMessage = "Email should not exceed 200 characters")]
        public string Email { get; set; }
        
        [StringLength(200, ErrorMessage = "Password should not exceed 200 characters")]
        public string Password { get; set; }
        
        [StringLength(200, ErrorMessage = "FirstName should not exceed 200 characters")]
        public string FirstName { get; set; }
        
        [StringLength(200, ErrorMessage = "LastName should not exceed 200 characters")]
        public string LastName { get; set; }
        
        [StringLength(200, ErrorMessage = "Address should not exceed 200 characters")]
        public string Address { get; set; }
        
        [StringLength(200, ErrorMessage = "City should not exceed 200 characters")]
        public string City { get; set; }
        
        [StringLength(200, ErrorMessage = "Country should not exceed 200 characters")]
        public string Country { get; set; }
        
        [StringLength(200, ErrorMessage = "ZipCode should not exceed 200 characters")]
        public int? ZipCode { get; set; }
    }
}