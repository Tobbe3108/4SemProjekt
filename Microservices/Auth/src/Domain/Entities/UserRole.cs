using System;
using Auth.Domain.Common;

namespace Auth.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}