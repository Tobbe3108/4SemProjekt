using System.Threading.Tasks;
using ToolBox.Commands;
using ToolBox.Events;

namespace ToolBox.Bus.Interfaces
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void PublishEvent<T>(T @event) where T : Event;

        void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
    }
}