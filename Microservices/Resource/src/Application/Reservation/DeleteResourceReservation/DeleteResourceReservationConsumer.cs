using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Reservation.DeleteResourceReservation
{
    public class DeleteResourceReservationConsumer : IConsumer<ToolBox.Contracts.Resource.DeleteResourceReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<ToolBox.Contracts.Resource.DeleteResourceReservation> _logger;

        public DeleteResourceReservationConsumer(IApplicationDbContext dbContext, ILogger<ToolBox.Contracts.Resource.DeleteResourceReservation> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.DeleteResourceReservation> context)
        {
            _logger.LogInformation("DeleteResourceReservationConsumer Called"); 
            
            var reservationToDelete = await _dbContext.Reservations.FindAsync(context.Message.Id);

            var resource = await _dbContext.Resources.FindAsync(reservationToDelete.ResourceId);
            resource.Reservations.Remove(reservationToDelete);
            _dbContext.Reservations.Remove(reservationToDelete);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<ResourceReservationDeleted>(new
            {
                context.Message.Id,
            });
        }
    }
}