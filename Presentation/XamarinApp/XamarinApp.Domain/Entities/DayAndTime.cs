using System;

namespace XamarinApp.Domain.Entities
{
    public class DayAndTime
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
    }
}