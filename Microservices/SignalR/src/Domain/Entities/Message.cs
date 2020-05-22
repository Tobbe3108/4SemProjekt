using System;
using Syncfusion.XForms.Chat;

namespace SignalR.Domain.Entities
{
    public class Message : TextMessage
    {
        public Guid Id { get; set; }
        public string ChatId { get; set; }
    }
}