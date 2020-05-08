using System;
using ToolBox.Events;

namespace User.Domain.Entities
{
    public class OutboxMessage
    {
        public OutboxMessage(DateTime createdAt, BaseEvent @event)
        {
            CreatedAt = createdAt;
            Event = @event;
        }

        public Guid Id { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public BaseEvent Event { get; private set; }
    }
}