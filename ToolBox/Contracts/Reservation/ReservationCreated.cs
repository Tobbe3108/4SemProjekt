using System;

namespace ToolBox.Contracts.Reservation
{
    public interface ReservationCreated
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ResourceId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}