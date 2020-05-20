using System;

namespace ToolBox.Contracts.Reservation
{
    public interface GetReservationFromUser
    {
        public Guid UserId { get; set; }
    }
}