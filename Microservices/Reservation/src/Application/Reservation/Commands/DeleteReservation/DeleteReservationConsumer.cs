using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reservation.Application.Common.Interfaces;
using ToolBox.Contracts.Reservation;

namespace Reservation.Application.Reservation.Commands.DeleteReservation
{
    public class DeleteReservationConsumer : IConsumer<ToolBox.Contracts.Reservation.DeleteReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<DeleteReservationConsumer> _logger;

        public DeleteReservationConsumer(IApplicationDbContext dbContext, ILogger<DeleteReservationConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.DeleteReservation> context)
        {
            _logger.LogInformation("DeleteReservationConsumer Called");

            var reservationToDelete = await _dbContext.Reservations.FindAsync(context.Message.Id);

            _dbContext.Reservations.Remove(reservationToDelete);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<ReservationDeleted>(new
            {
                reservationToDelete.Id
            });
        }
    }
}