using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Reservation.UpdateResourceReservation
{
    public class UpdateResourceReservationConsumer : IConsumer<ToolBox.Contracts.Resource.UpdateResourceReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<ToolBox.Contracts.Resource.UpdateResourceReservation> _logger;

        public UpdateResourceReservationConsumer(IApplicationDbContext dbContext, ILogger<ToolBox.Contracts.Resource.UpdateResourceReservation> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.UpdateResourceReservation> context)
        {
            _logger.LogInformation("UpdateResourceReservationConsumer Called"); 
            
            var reservationToUpdate = await _dbContext.Reservations.FindAsync(context.Message.Id);

            reservationToUpdate.From = context.Message.From;
            reservationToUpdate.To = context.Message.To;
            
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<ResourceReservationUpdated>(new
            {
                context.Message.Id,
            });
        }
    }
}