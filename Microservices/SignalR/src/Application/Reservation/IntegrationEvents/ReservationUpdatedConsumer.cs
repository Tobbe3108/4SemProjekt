using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Enums;

namespace SignalR.Application.Reservation.IntegrationEvents
{
    public class ReservationUpdatedConsumer : IConsumer<ToolBox.Contracts.Reservation.ReservationUpdated>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationUpdatedConsumer> _logger;

        public ReservationUpdatedConsumer(IReservationService reservationService, ILogger<ReservationUpdatedConsumer> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.ReservationUpdated> context)
        {
            _logger.LogInformation("ReservationUpdatedConsumer Called");
            await _reservationService.SendReservation(Type.Update, new Domain.Entities.Reservation
            {
                Id = context.Message.Id,
                UserId = context.Message.UserId,
                ResourceId = context.Message.ResourceId,
                From = context.Message.From,
                To = context.Message.To
            });
            await Task.CompletedTask;
        }
    }
}