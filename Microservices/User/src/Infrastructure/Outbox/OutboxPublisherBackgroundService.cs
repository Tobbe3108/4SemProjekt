using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace User.Infrastructure.Outbox
{
    public class OutboxPublisherBackgroundService : BackgroundService
    {
        private readonly OutboxPublisher _publisher;
        private readonly OutboxListener _listener;
        private readonly ILogger<OutboxPublisherBackgroundService> _logger;

        public OutboxPublisherBackgroundService(OutboxPublisher publisher, OutboxListener listener, ILogger<OutboxPublisherBackgroundService> logger)
        {
            _publisher = publisher;
            _listener = listener;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var messageId in _listener.GetAllMessageIdsAsync(stoppingToken))
            {
                try
                {
                    await _publisher.PublishAsync(messageId, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Unexpected error while publishing pending outbox messages");
                }
            }
        }
    }
}