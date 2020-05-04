using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToolBox.Bus;
using ToolBox.Bus.Interfaces;
using ToolBox.Events;
using ToolBox.Repository;
using ToolBox.Service;

namespace ToolBox.IoC
{
    public static class Extensions
    {
        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMqBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqBus(sp.GetService<IMediator>(), scopeFactory);
            });
        }

        public static void Subscribe<T, TH>(this IApplicationBuilder app) where T : Event where TH : IEventHandler<T>
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<T, TH>();
        }

        public static void AddGenericRepository(this IServiceCollection services, Type type)
        {
            services.AddScoped(typeof(IRepository<>), type);
        }

        public static void AddGenericService(this IServiceCollection services, Type type)
        {
            services.AddScoped(typeof(IService<>), type);
        }
    }
}