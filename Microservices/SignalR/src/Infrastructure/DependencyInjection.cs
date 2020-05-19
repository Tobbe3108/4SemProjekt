using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Application.Common.Interfaces;
using SignalR.Infrastructure.Services;

namespace SignalR.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IDateTime, DateTimeService>();
            
            return services;
        }
    }
}