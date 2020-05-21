using System;
using Syncfusion.XForms.Chat;

namespace XamarinApp.Domain.Entities
{
    public class Message : TextMessage
    {
        public Guid Id { get; set; }
    }
}