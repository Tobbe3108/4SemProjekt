using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using User.Application.Common.Interfaces;

namespace Contracts.User
{
    public interface GetUser
    {
        public Guid Id { get; set; }
    }

    public class GetUserConsumer : IConsumer<GetUser>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetUserConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<GetUser> context)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == context.Message.Id);
   
                await context.RespondAsync<UserVm>(new
                {
                    User = user
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