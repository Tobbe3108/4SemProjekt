using System;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Reservation.Application.Common.Interfaces;
using Reservation.Application.Reservation.Commands.CreateReservation;
using Reservation.Application.Reservation.Commands.DeleteReservation;
using Reservation.Application.Reservation.Commands.UpdateReservation;
using Reservation.Application.Reservation.Queries.GetReservation;
using Reservation.Infrastructure;
using Reservation.Infrastructure.Persistence;
using Reservation.WebApi.Filters;
using Reservation.WebApi.Services;
using ToolBox.Contracts.Reservation;
using ToolBox.Contracts.Resource;
using ToolBox.Contracts.User;
using ToolBox.IoC;

namespace Reservation.WebApi
{
    public class Startup
    {
        private const string xmlKey =
            "<RSAKeyValue>" +
            "<Modulus>p9ZX2CSot2aHOiIRJJz0lngezY51Z+stl/sMYGFD1rxcYZbuHDs/cZgUURDhxdlkGoLGv5VSVSyecJ15LIDsjkaKeZ5HJOT5TXVXQOtvtq8Wm/gPsOZso0qoxNIswKwEAsHclfaNOQ7zi3yvVv04Wq3AnhC6y2u/I7YhZUIZtW9oy1BWKnP+HS0PUlP+EhCSmcCro76kWNTQn0Y9lv9ouJqrlOuGmjBEobCyGXISQYfitCTMFZXTcFv9k5F8Y3Kq7FIjAakAjX90rUzl5JxY81Q+8xeOT7zzXn+CrqGuFvlQ0+QrIJLylUOf/x6OguBHlfco682RIqReVFGRwPU+db77OUlj7Yazq1s5X2aRUFn+dRIo/x7+iEin+b1OeA8JycjCrk6bqkttGpy4rKYGuZfoheRwUoJdI8KnuWwWg7D5VbxCh0TX8l9aSczQCryHNN0YZtVDbxRhU/HdOgHSzTAzKsQ8O/fJwgGcaEZs/JH3AS9BGmfurYXZbpiMnkoBEvZpe1pd64GeRenaaCnL2UYFu96Bbb/IUW62foh78+T/leuY1buTLlsiYHAu2fmZw7FBiaPa+RSJ6WXO/sPG/aFPk3AgZx6xX/9tY7Zo1UJ4BWyNw3tpxM+NTu49y9rdiaJ1hdZscPfFACpt/VFFKolgMcVauqV+OvVBZO3ZrsE=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "</RSAKeyValue>";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ToolboxAddAuthentication(Configuration["Jwt:Issuer"], xmlKey);
            
            services.AddInfrastructure(Configuration);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            
            services.AddControllers();
            
            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Reservation Microservice", Version = "v1"});
                c.ToolboxAddSwaggerSecurity();
                c.SchemaFilter<SchemaFilter>();
            });
            
            #region MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<CreateReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<UpdateReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<DeleteReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<GetReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<GetReservationFromUserConsumer>();
                
                x.AddRequestClient<GetReservation>();
                x.AddRequestClient<GetReservationFromUser>();
                x.AddRequestClient<SubmitReservation>();
                
                x.AddBus(ConfigureBus);
                x.AddTransactionOutbox();
            });

            services.AddMassTransitHostedService();
            #endregion
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reservation Microservice v1"));
        }
        
        static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ConfigureEndpoints(provider, KebabCaseEndpointNameFormatter.Instance);
                cfg.ReceiveEndpoint("submit-reservation", e =>
                {
                    e.StateMachineSaga(new CreateReservationStateMachine(), new InMemorySagaRepository<CreateReservationState>());
                    e.UseInMemoryOutbox();
                });
                cfg.ReceiveEndpoint("submit-delete-reservation", e =>
                {
                    e.StateMachineSaga(new DeleteReservationStateMachine(), new InMemorySagaRepository<DeleteReservationState>());
                    e.UseInMemoryOutbox();
                });
                cfg.ReceiveEndpoint("submit-update-reservation", e =>
                {
                    e.StateMachineSaga(new UpdateReservationStateMachine(), new InMemorySagaRepository<UpdateReservationState>());
                    e.UseInMemoryOutbox();
                });
            });
        }
    }
}