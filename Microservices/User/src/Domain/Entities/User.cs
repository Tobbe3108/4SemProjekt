using System;
using System.ComponentModel.DataAnnotations.Schema;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? ZipCode { get; set; }
        [NotMapped] public string Password { get; set; }
    }
}