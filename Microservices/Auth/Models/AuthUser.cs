using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Api.Models
{
    [Serializable]
    public class AuthUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Email { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
