using System;
using System.Drawing;
using System.Threading.Tasks;
using MassTransit;
using ToolBox.Contracts.User;

namespace User.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        public async Task SeedSampleDataAsync(IRequestClient<SubmitUser> requestClient)
        {
            await requestClient.GetResponse<SubmitUserAccepted>(new
            {
                Id = Guid.NewGuid(),
                Username = "Admin",
                Email = "Admin@company.com",
                FirstName = "Admin",
                LastName = "Superuser",
                Password = "Zxasqw12"
            });
            
            await requestClient.GetResponse<SubmitUserAccepted>(new
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