using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reservation.Application.Common.Interfaces;
using ToolBox.Contracts.Reservation;

namespace Reservation.Application.Reservation.Commands.UpdateReservation
{
    public class UpdateReservationConsumer : IConsumer<ToolBox.Contracts.Reservation.UpdateReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateReservationConsumer> _logger;

        public UpdateReservationConsumer(IApplicationDbContext dbContext, ILogger<UpdateReservationConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.UpdateReservation> context)
        {
            _logger.LogInformation("UpdateReservationConsumer Called");

            var reservationToUpdate = await _dbContext.Reservations.FindAsync(context.Message.Id);

            reservationToUpdate.From = context.Message.From;
            reservationToUpdate.To = context.Message.To;

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<ReservationUpdated>(new
            {
                reservationToUpdate.Id,
                reservationToUpdate.UserId,
                reservationToUpdate.ResourceId,
                reservationToUpdate.From,
                reservationToUpdate.To
            });
        }
    }
}