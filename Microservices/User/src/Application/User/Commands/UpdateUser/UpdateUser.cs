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