using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Domain.Entities;

namespace Auth.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            var roles = new List<string>
            {
                "Admin",
                "CustomerService"
            };

            foreach (var role in roles)
            {
                //creating the role and seeding it to the database
                var roleExist = context.Roles.Any(r => r.RoleName == role);

                var roleToAdd = new Role
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = role
                };

                if (!roleExist) await context.Roles.AddAsync(roleToAdd);
            }
            
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}