using System;
using NodaTime;

namespace Resource.Domain.Entities
{
    public class DayAndTime
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public LocalTime To { get; set; }
        public LocalTime From { get; set; }
    }
}