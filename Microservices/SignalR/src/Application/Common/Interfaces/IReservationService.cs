using System.Threading.Tasks;

namespace SignalR.Application.Common.Interfaces
{
    public interface IReservationService
    {
        Task SendReservation(Domain.Entities.Reservation reservation);
    }
}