using System;

namespace ToolBox.Events
{
    public abstract class Event
    {
        protected Event()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; protected set; }
    }
}