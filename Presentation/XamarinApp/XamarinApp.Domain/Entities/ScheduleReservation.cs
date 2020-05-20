using System;
using Syncfusion.SfSchedule.XForms;

namespace XamarinApp.Domain.Entities
{
    public class ScheduleReservation : ScheduleAppointment
    {
        public Guid ReservationId { get; set; }
    }
}