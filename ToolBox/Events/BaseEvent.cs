using System;

namespace ToolBox.Events
{
    public abstract class BaseEvent
    {
        protected BaseEvent()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; protected set; }
    }
}