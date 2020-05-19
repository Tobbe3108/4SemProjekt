using System;

namespace ToolBox.Contracts.Reservation
{
    public interface SubmitUpdateReservation
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}