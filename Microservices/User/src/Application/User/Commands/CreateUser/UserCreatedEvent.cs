using System;
using ToolBox.Events;

namespace User.Application.User.Commands.CreateUser
{
    public class UserCreatedEvent : BaseEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}