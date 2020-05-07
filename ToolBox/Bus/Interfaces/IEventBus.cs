using ToolBox.Events;

namespace ToolBox.Bus.Interfaces
{
    public interface IEventBus
    {
        void PublishEvent<T>(T @event) where T : BaseEvent;

        void Subscribe<T, TH>() where T : BaseEvent where TH : IEventHandler<T>;
    }
}