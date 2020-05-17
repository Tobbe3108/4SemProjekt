using System;
using NodaTime;

namespace BlazorSPA.Client.Data
{
    public class DayAndTime
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
    }
}