using System;
using System.Collections.Generic;
using System.Linq;
using Auth.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Auth.Api.Infrastructure;
using Auth.Api.Infrastructure.Services;
using Auth.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api
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
            services.AddControllers();
            services.AddScoped<AuthService>();
            services.AddScoped<HashService>();
            services.AddScoped<AuthRepository>();
            services.AddDbContext<AuthDbContext>();
            services.AddMediatR(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Auth MicroService", Version = "v1"});
                c.SchemaFilter<SchemaFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/v1/swagger.json", "Auth API V1"); });
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)

        {
            //adding custom roles

            var authContext = serviceProvider.GetRequiredService<AuthDbContext>();
            var hashService = serviceProvider.GetRequiredService<HashService>();

            var roleName = "Admin";

            //creating the role and seeding it to the database
            var roleExist = authContext.Roles.Any(r => r.RoleName == roleName);

            var roleToAdd = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName
            };

            if (!roleExist) authContext.Roles.Add(roleToAdd);

            var _user = authContext.AuthUsers.SingleOrDefault(u =>
                u.UserName == Configuration.GetSection("UserSettings")["UserName"]);

            if (_user == null)
            {
                //creating an admin
                var admin = new AuthUser
                {
                    Id = Guid.NewGuid(),
                    UserName = Configuration.GetSection("UserSettings")["UserName"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"]
                };

                admin.PasswordSalt = hashService.GenerateSalt();

                admin.PasswordHash = hashService.GenerateHash(Configuration.GetSection("UserSettings")["UserPassword"],
                    admin.PasswordSalt);

                authContext.AuthUsers.Add(admin);
                authContext.UserRoles.Add(new UserRole {AuthUserId = admin.Id, Role = roleToAdd});
                authContext.SaveChanges();
            }
        }
    }
}