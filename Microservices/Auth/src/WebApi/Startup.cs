using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Application;
using Auth.Application.Common.Interfaces;
using Auth.Application.User.IntegrationEvents.UserCreated;
using Auth.Application.User.IntegrationEvents.UserDeleted;
using Auth.Application.User.IntegrationEvents.UserUpdated;
using Auth.Domain.Entities;
using Auth.Infrastructure;
using Auth.Infrastructure.Persistence;
using Auth.WebApi.Filters;
using Auth.WebApi.Services;
using Contracts.AuthUser;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ToolBox.IoC;


namespace Auth.WebApi
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
            
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.ToolboxAddRabbitMq();
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<UserCreatedEventHandler>();
            services.AddTransient<UserUpdatedEventHandler>();
            services.AddTransient<UserDeletedEventHandler>();

            services.AddHttpContextAccessor();
            
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Auth MicroService", Version = "v1"});
                c.ToolboxAddSwaggerSecurity();
                c.SchemaFilter<SchemaFilter>();
            });
            
            #region MassTransit
            // Consumer dependencies should be scoped
            //services.AddScoped<SomeConsumerDependency>();
            
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<CreateAuthUserConsumer>();
                x.AddBus(ConfigureBus);
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
            
            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Microservice v1"));
            
            await CreateRoles(serviceProvider);
            
            // app.ToolboxSubscribe<UserCreatedEvent, UserCreatedEventHandler>();
            // app.ToolboxSubscribe<UserUpdatedEvent, UserUpdatedEventHandler>();
            // app.ToolboxSubscribe<UserDeletedEvent, UserDeletedEventHandler>();
        }
        
        static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://localhost");

                cfg.ConfigureEndpoints(provider);
            });
        }
        
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles

            var authContext = serviceProvider.GetRequiredService<IApplicationDbContext>();
            var hashService = serviceProvider.GetRequiredService<IHashService>();

            const string roleName = "Admin";

            //creating the role and seeding it to the database
            var roleExist = authContext.Roles.Any(r => r.RoleName == roleName);

            var roleToAdd = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName
            };

            if (!roleExist) await authContext.Roles.AddAsync(roleToAdd);

            var user = authContext.Users.SingleOrDefault(u =>
                u.UserName == Configuration.GetSection("UserSettings")["UserName"]);

            if (user == null)
            {
                //creating an admin
                var admin = new AuthUser
                {
                    Id = Guid.NewGuid(),
                    UserName = Configuration.GetSection("UserSettings")["UserName"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"],
                    PasswordSalt = hashService.GenerateSalt()
                };


                admin.PasswordHash = hashService.GenerateHash(Configuration.GetSection("UserSettings")["UserPassword"],
                    admin.PasswordSalt);

                await authContext.Users.AddAsync(admin);
                await authContext.UserRoles.AddAsync(new UserRole {UserId = admin.Id, Role = roleToAdd});
                await authContext.SaveChangesAsync(CancellationToken.None);
            }
        }
    }
}