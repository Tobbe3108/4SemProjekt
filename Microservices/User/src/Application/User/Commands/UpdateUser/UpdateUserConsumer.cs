using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using ToolBox.Contracts.User;
using User.Application.Common.Interfaces;

namespace User.Application.User.Commands.UpdateUser
{
    public class UpdateUserConsumer : IConsumer<ToolBox.Contracts.User.UpdateUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateUserConsumer> _logger;

        public UpdateUserConsumer(IApplicationDbContext dbContext, ILogger<UpdateUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.User.UpdateUser> context)
        {
            _logger.LogInformation("UpdateUserConsumer Called");
            
            var entity = await _dbContext.Users.FindAsync(context.Message.Id);

            if (context.Message.Address != null) entity.Address = context.Message.Address;
            if (context.Message.City != null) entity.City = context.Message.City;
            if (context.Message.Country != null) entity.Country = context.Message.Country;
            if (context.Message.Email != null) entity.Email = context.Message.Email;
            if (context.Message.Username != null)
            {
                entity.Username = context.Message.Username;
                entity.NormalizedUserName = context.Message.Username.ToUpperInvariant();
            }
            if (context.Message.FirstName != null) entity.FirstName = context.Message.FirstName;
            if (context.Message.LastName != null) entity.LastName = context.Message.LastName;
            if (context.Message.ZipCode != null) entity.ZipCode = context.Message.ZipCode;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<UserUpdated>(new
            {
                context.Message.Id
            });
        }
    }
}