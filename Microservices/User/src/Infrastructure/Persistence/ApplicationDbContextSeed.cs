using System;
using System.Drawing;
using System.Threading.Tasks;
using MassTransit;
using ToolBox.Contracts.User;

namespace User.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(IRequestClient<SubmitUser> requestClient)
        {
            var response = requestClient.GetResponse<SubmitUserAccepted>(new
            {
                Id = Guid.NewGuid(),
                Username = "Admin",
                Email = "Admin@company.com",
                FirstName = "Admin",
                LastName = "Superuser",
                Password = "Zxasqw12"
            });
            
            response = requestClient.GetResponse<SubmitUserAccepted>(new
            {
                Id = Guid.NewGuid(),
                Username = "CustomerService",
                Email = "Customer@service.com",
                FirstName = "Customer",
                LastName = "Service",
                Password = "Zxasqw12"
            });
        } 
    }
}