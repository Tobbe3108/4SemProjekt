using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts.Auth;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Reservation.CreateResourceReservation
{
    public class CreateResourceReservationConsumer : IConsumer<ToolBox.Contracts.Resource.CreateResourceReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateResourceReservationConsumer> _logger;

        public CreateResourceReservationConsumer(IApplicationDbContext dbContext, ILogger<CreateResourceReservationConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.CreateResourceReservation> context)
        {
            _logger.LogInformation("CreateResourceReservationConsumer Called"); 
            
            var reservationToCreate = new Domain.Entities.Reservation
            {
                Id = context.Message.Id,
                UserId = context.Message.UserId,
                ResourceId = context.Message.ResourceId,
                From = context.Message.From,
                To = context.Message.To
            };

            var resource = await _dbContext.Resources.FirstOrDefaultAsync(r => r.Id == reservationToCreate.ResourceId);

            if (resource != null)
            {
                resource.Reservations ??= new List<Domain.Entities.Reservation>();
                resource.Reservations.Add(reservationToCreate);
                await _dbContext.Reservations.AddAsync(reservationToCreate);
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                await context.Publish<ResourceReservationCreated>(new
                {
                    reservationToCreate.Id
                });
            }
        }
    }
}