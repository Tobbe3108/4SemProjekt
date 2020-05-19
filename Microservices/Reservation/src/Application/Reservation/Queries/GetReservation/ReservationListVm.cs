using System.Collections.Generic;

namespace Reservation.Application.Reservation.Queries.GetReservation
{
    public interface ReservationListVm
    {
        public List<global::Reservation.Domain.Entities.Reservation> Reservation { get; set; }
    }
}