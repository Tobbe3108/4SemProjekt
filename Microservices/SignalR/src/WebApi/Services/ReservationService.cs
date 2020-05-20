using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Entities;
using SignalR.WebApi.Hubs;
using Type = SignalR.Domain.Enums.Type;

namespace SignalR.WebApi.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IHubContext<ReservationHub> _hubContext;

        public ReservationService(IHubContext<ReservationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public async Task SendReservation(Type type, Reservation reservation)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveReservation", type, reservation);
        }
    }
}