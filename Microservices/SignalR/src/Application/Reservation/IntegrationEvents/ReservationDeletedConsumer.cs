using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using SignalR.Application.Common.Interfaces;
using SignalR.Domain.Enums;

namespace SignalR.Application.Reservation.IntegrationEvents
{
    public class ReservationDeletedConsumer : IConsumer<ToolBox.Contracts.Reservation.ReservationDeleted>
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationDeletedConsumer> _logger;

        public ReservationDeletedConsumer(IReservationService reservationService, ILogger<ReservationDeletedConsumer> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.ReservationDeleted> context)
        {
            _logger.LogInformation("ReservationDeletedConsumer Called");
            await _reservationService.SendReservation(Type.Delete, new Domain.Entities.Reservation
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