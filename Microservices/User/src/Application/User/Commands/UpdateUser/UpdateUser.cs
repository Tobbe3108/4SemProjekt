using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.User;
using User.Application.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Contracts.User
{
    public interface UpdateUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? ZipCode { get; set; }
    }

    public class UpdateUserConsumer : IConsumer<Contracts.User.UpdateUser>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<UpdateUserConsumer> _logger;

        public UpdateUserConsumer(IApplicationDbContext dbContext, ILogger<UpdateUserConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<Contracts.User.UpdateUser> context)
        {
            _logger.LogInformation("UpdateUserConsumer Called");
            
            var entity = await _dbContext.Users.FindAsync(context.Message.Id);

            entity.Address = context.Message.Address;
            entity.City = context.Message.City;
            entity.Country = context.Message.Country;
            entity.Email = context.Message.Email;
            entity.Username = context.Message.Email;
            entity.NormalizedUserName = context.Message.Username.ToUpperInvariant();
            entity.FirstName = context.Message.FirstName;
            entity.LastName = context.Message.LastName;
            entity.ZipCode = context.Message.ZipCode;
            
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            await context.Publish<UserUpdated>(new
            {
                context.Message.Id
            });
        }
    }
}