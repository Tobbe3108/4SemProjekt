﻿﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Reservation.Domain.Common;

namespace Reservation.Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ResourceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}