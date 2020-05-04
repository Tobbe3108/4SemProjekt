using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ToolBox.Bus.Interfaces;
using ToolBox.Commands;
using ToolBox.Events;

namespace ToolBox.Bus
{
    public sealed class RabbitMqBus : IEventBus
    {
        private readonly List<Type> _eventTypes;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void PublishEvent<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(@"amqp://uuwjhqtb:FIaXRCFlumGxOENo0TGLM-M8c6T850uK@hawk.rmq.cloudamqp.com/uuwjhqtb")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.ExchangeDeclare(eventName, ExchangeType.Fanout);
            channel.BasicPublish(eventName, "", null, body);
        }

        public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T))) _eventTypes.Add(typeof(T));
            if (!_handlers.ContainsKey(eventName)) _handlers.Add(eventName, new List<Type>());
            if (_handlers[eventName].Any(x => x.GetType() == handlerType))
                throw new ArgumentException($"Handler Type {handlerType.Name} is already registered for {eventName}",
                    nameof(handlerType));

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(@"amqp://uuwjhqtb:FIaXRCFlumGxOENo0TGLM-M8c6T850uK@hawk.rmq.cloudamqp.com/uuwjhqtb")
            };


            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var eventName = typeof(T).Name;
            var queueName = channel.QueueDeclare(eventName + "-" + Guid.NewGuid(), false, false, true, null).QueueName;

            channel.ExchangeDeclare(eventName, ExchangeType.Fanout);
            channel.QueueBind(queueName, eventName, "");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queueName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.Exchange;
            var message = Encoding.UTF8.GetString(@event.Body.Span);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventName];
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subscription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task) concreteType.GetMethod("Handle").Invoke(handler, new[] {@event});
                    }
                }
        }
    }
}