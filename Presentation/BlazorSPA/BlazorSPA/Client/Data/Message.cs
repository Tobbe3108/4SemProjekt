using System;
using Syncfusion.XForms.Chat;

namespace BlazorSPA.Client.Data
{
    public class Message  : TextMessage
    {
        public Guid Id { get; set; }
        public string ChatId { get; set; }
    }
}