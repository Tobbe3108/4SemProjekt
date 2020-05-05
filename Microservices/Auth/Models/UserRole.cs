using System;
using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models
{
    public class UserRole
    {
        public Guid AuthUserId { get; set; }
        //public AuthUser AuthUser { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
