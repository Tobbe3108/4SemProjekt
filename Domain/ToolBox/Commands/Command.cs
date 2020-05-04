using System;
using ToolBox.Events;

namespace ToolBox.Commands
{
    public abstract class Command : Message
    {
        protected Command()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; protected set; }
    }
}