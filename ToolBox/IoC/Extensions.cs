using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToolBox.Bus;
using ToolBox.Bus.Interfaces;
using ToolBox.Events;

namespace ToolBox.IoC
{
    public static class Extensions
    {
        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMqBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqBus(scopeFactory);
            });
        }

        public static void Subscribe<T, TH>(this IApplicationBuilder app) where T : BaseEvent where TH : IEventHandler<T>
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<T, TH>();
        }
    }
}