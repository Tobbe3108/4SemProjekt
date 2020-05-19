using System;
using System.Reflection;
using Automatonymous;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Resource.Application.Common.Interfaces;
using Resource.Application.Reservation.CreateResourceReservation;
using Resource.Application.Reservation.DeleteResourceReservation;
using Resource.Application.Reservation.UpdateResourceReservation;
using Resource.Application.Resource.Commands.CreateResource;
using Resource.Application.Resource.Commands.DeleteResource;
using Resource.Application.Resource.Commands.UpdateResource;
using Resource.Application.Resource.Queries.GetResource;
using Resource.Infrastructure;
using Resource.Infrastructure.Persistence;
using Resource.WebApi.Filters;
using Resource.WebApi.Services;
using ToolBox.Contracts.Resource;
using ToolBox.IoC;

namespace Resource.WebApi
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

            services.AddCors();
            services.AddControllers();
            
            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Resource Microservice", Version = "v1"});
                c.ToolboxAddSwaggerSecurity();
                c.SchemaFilter<SchemaFilter>();
            });
            
            #region MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<CreateResourceConsumer>();
                x.AddConsumersFromNamespaceContaining<UpdateResourceConsumer>();
                x.AddConsumersFromNamespaceContaining<DeleteResourceConsumer>();
                x.AddConsumersFromNamespaceContaining<GetAllResourcesConsumer>();
                x.AddConsumersFromNamespaceContaining<CreateResourceReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<UpdateResourceReservationConsumer>();
                x.AddConsumersFromNamespaceContaining<DeleteResourceReservationConsumer>();

                x.AddRequestClient<SubmitResource>();
                x.AddRequestClient<GetResource>();
                x.AddRequestClient<GetAllResources>();
                
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

            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resource Microservice v1"));
        }
        
        static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ConfigureEndpoints(provider, KebabCaseEndpointNameFormatter.Instance);
                cfg.ReceiveEndpoint("submit-resource", e =>
                {
                    e.StateMachineSaga(new CreateResourceStateMachine(), new InMemorySagaRepository<CreateResourceState>());
                    e.UseInMemoryOutbox();
                });
                cfg.ReceiveEndpoint("submit-delete-resource", e =>
                {
                    e.StateMachineSaga(new DeleteResourceStateMachine(), new InMemorySagaRepository<DeleteResourceState>());
                    e.UseInMemoryOutbox();
                });
                cfg.ReceiveEndpoint("submit-update-resource", e =>
                {
                    e.StateMachineSaga(new UpdateResourceStateMachine(), new InMemorySagaRepository<UpdateResourceState>());
                    e.UseInMemoryOutbox();
                });
            });
        }
    }
}