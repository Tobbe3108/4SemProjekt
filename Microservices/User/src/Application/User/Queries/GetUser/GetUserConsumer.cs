using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ToolBox.Contracts;
using ToolBox.Contracts.User;
using User.Application.Common.Interfaces;

namespace User.Application.User.Queries.GetUser
{
    public class GetUserConsumer : IConsumer<ToolBox.Contracts.User.GetUser>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetUserConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.User.GetUser> context)
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