using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Reservation.Application.Common.Interfaces;
using ToolBox.Contracts;

namespace Reservation.Application.Reservation.Queries.GetReservation
{
    public class GetReservationFromUserConsumer : IConsumer<ToolBox.Contracts.Reservation.GetReservationFromUser>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetReservationFromUserConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Reservation.GetReservationFromUser> context)
        {
            try
            {
                var reservations = await _dbContext.Reservations
                    .Where(u => u.UserId == context.Message.UserId).ToListAsync();
            
                await context.RespondAsync<ReservationListVm>(new
                {
                    Reservations = reservations
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