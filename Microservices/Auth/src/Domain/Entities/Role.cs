using System;
using Auth.Domain.Common;

namespace Auth.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
    }
}