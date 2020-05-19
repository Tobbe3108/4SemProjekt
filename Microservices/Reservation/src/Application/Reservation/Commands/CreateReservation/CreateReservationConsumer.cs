using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reservation.Application.Common.Interfaces;
using ToolBox.Contracts.Reservation;
using ToolBox.Contracts.User;

namespace Reservation.Application.Reservation.Commands.CreateReservation
{
    public class CreateReservationConsumer : IConsumer<ToolBox.Contracts.Reservation.CreateReservation>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<CreateReservationConsumer> _logger;

        public CreateReservationConsumer(IApplicationDbContext dbContext, ILogger<CreateReservationConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.CreateReservation> context)
        {
            _logger.LogInformation("CreateReservationConsumer Called");

            var entity = new global::Reservation.Domain.Entities.Reservation
            {
                Id = context.Message.Id,
                UserId = context.Message.UserId,
                ResourceId = context.Message.ResourceId,
                From = context.Message.From,
                To = context.Message.To
            };
            
            await _dbContext.Reservations.AddAsync(entity);

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            await context.Publish<ReservationCreated>(new
            {
                entity.Id,
                entity.UserId,
                entity.ResourceId,
                entity.From,
                entity.To
            });
        }
    }
}