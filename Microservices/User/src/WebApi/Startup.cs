using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToolBox.IoC;
using User.Application;
using User.Application.Common.Interfaces;
using User.Infrastructure;
using User.Infrastructure.Persistence;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi
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

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
            
            services.AddControllers();
            
            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "User Microservice", Version = "v1"});
                c.ToolboxAddSwaggerSecurity();
                c.SchemaFilter<SchemaFilter>();
            });
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Microservice v1"));
        }
    }
}