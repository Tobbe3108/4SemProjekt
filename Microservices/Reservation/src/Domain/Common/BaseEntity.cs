using System;

namespace Reservation.Domain.Common
{
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}