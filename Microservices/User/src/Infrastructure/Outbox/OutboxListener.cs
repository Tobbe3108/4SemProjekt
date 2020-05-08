using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace User.Infrastructure.Outbox
{
    public class OutboxListener
    {
        private readonly ILogger<OutboxListener> _logger;
        private readonly Channel<Guid> _messageIdChannel;

        public OutboxListener(ILogger<OutboxListener> logger)
        {
            _logger = logger;
            _messageIdChannel = Channel.CreateUnbounded<Guid>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });
        }

        public void OnNewMessages(IEnumerable<Guid> messageIds)
        {
            foreach (var messageId in messageIds)
            {
                if (!_messageIdChannel.Writer.TryWrite(messageId) && _logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug($"Could not add outbox message {messageId} to the channel");
                }
            }
        }

        public IAsyncEnumerable<Guid> GetAllMessageIdsAsync(CancellationToken ct) =>
            _messageIdChannel.Reader.ReadAllAsync(ct);
    }
}