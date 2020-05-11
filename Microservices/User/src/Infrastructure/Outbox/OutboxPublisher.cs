using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToolBox.Bus.Interfaces;
using User.Domain.Entities;
using User.Infrastructure.Persistence;

namespace User.Infrastructure.Outbox
{
    public class OutboxPublisher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IEventBus _eventBus;
        private readonly ILogger<OutboxPublisher> _logger;

        public OutboxPublisher(IServiceScopeFactory serviceScopeFactory, IEventBus eventBus, ILogger<OutboxPublisher> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _eventBus = eventBus;
            _logger = logger;
        }
        
        public async Task PublishAsync(Guid messageId, CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            //await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken); //Transactions NOT supported in InMemoryDb
            try
            {
                var message = await dbContext.Set<OutboxMessage>().FindAsync(new object[] {messageId}, stoppingToken);

                if (await TryDeleteMessageAsync(dbContext, message, stoppingToken))
                {
                    //_logger.LogInformation($"Event with Name: {message.Event.GetType().Name} (outbox message Id {messageId}) published -> {Newtonsoft.Json.JsonConvert.SerializeObject(message.Event)}");
                    //_eventBus.PublishEvent(message.Event);
                    //await transaction.CommitAsync(stoppingToken); //Transactions NOT supported in InMemoryDb
                }
                else
                {
                    //_logger.LogInformation($"Event with Name: {message.Event.GetType().Name} (outbox message Id {messageId}) Failed to publish");
                    //await transaction.RollbackAsync(stoppingToken); //Transactions NOT supported in InMemoryDb
                }
            }
            catch (Exception e)
            {
                //await transaction.RollbackAsync(stoppingToken); //Transactions NOT supported in InMemoryDb
                //_logger.LogWarning(e, e.Message);
            }
        }

        private async Task<bool> TryDeleteMessageAsync(ApplicationDbContext dbContext, OutboxMessage message, CancellationToken stoppingToken)
        {
            try
            {
                dbContext.Set<OutboxMessage>().Remove(message);
                await dbContext.SaveChangesAsync(stoppingToken);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogDebug($"Delete message {message.Id} failed, as it was done concurrently");
                return false;
            }
        }
    }
}