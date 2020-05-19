using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Reservation.Application.Common.Interfaces;
using ToolBox.Contracts;

namespace Reservation.Application.Reservation.Queries.GetReservation
{
    public class GetReservationConsumer : IConsumer<ToolBox.Contracts.Reservation.GetReservation>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetReservationConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.GetReservation> context)
        {
            try
            {
                var reservation = await _dbContext.Reservations
                    .FirstOrDefaultAsync(u => u.Id == context.Message.Id);
            
                await context.RespondAsync<ReservationVm>(new
                {
                    Reservation = reservation
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync<NotFound>(new
                {
                    Message = e.Message
                });
            }
        }
    }
}