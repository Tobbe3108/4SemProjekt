using System.Threading.Tasks;
using Type = SignalR.Domain.Enums.Type;

namespace SignalR.Application.Common.Interfaces
{
    public interface IReservationService
    {
        Task SendReservation(Type type, Domain.Entities.Reservation reservation);
    }
}