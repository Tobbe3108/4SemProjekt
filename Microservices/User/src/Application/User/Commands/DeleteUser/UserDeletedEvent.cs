using System;
using ToolBox.Events;

namespace User.Application.User.Commands.DeleteUser
{
    public class UserDeletedEvent : BaseEvent
    {
        public Guid Id { get; set; }
    }
}