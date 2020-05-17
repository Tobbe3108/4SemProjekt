using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorSPA.Client.Data
{
    public class ResourceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
        
        public bool HasOverlapping(DayAndTime timeSlot)
        {

            foreach (var dayAndTime in Available)
            {
                var one = timeSlot.From;
                var two = dayAndTime.To;
                var tre = dayAndTime.From;
                var four = timeSlot.To;

                if (dayAndTime.DayOfWeek == timeSlot.DayOfWeek)
                {
                    if (dayAndTime.Id != timeSlot.Id)
                    {
                        if (timeSlot.From < dayAndTime.To && dayAndTime.From < timeSlot.To) return true;
                    }
                }
            }

            return false;
        }
    }
}