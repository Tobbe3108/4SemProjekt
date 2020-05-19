using System;
using System.Linq;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.Application.Common.Interfaces;
using SignalR.Application.Reservation.IntegrationEvents;
using SignalR.Application.Resource.IntegrationEvents;
using SignalR.Infrastructure;
using SignalR.WebApi.Hubs;
using SignalR.WebApi.Services;

namespace SignalR.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            
            services.AddInfrastructure(Configuration);
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IReservationService, ReservationService>();
            
            services.AddHttpContextAccessor();

            services.AddCors();
            
            services.AddControllers();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            
            #region MassTransit

            // Consumer dependencies should be scoped
            //services.AddScoped<SomeConsumerDependency>();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<ResourceCreatedConsumer>();
                x.AddConsumersFromNamespaceContaining<ReservationCreatedConsumer>();
                x.AddBus(ConfigureBus);
                x.AddTransactionOutbox();
            });

            services.AddMassTransitHostedService();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseHealthChecks("/health");
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ResourceHub>("/resourceHub");
                endpoints.MapHub<ReservationHub>("/reservationHub");
            });
        }

        static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ConfigureEndpoints(provider, KebabCaseEndpointNameFormatter.Instance);
            });
        }
    }
}