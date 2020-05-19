using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.Application.Common.Interfaces;
using Reservation.Infrastructure.Persistence;
using Reservation.Infrastructure.Services;

namespace Reservation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("UserDb"));
            else
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IDateTime, DateTimeService>();
            
            return services;
        }
    }
}