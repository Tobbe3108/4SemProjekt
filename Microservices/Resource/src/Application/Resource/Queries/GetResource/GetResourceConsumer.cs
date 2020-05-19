using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts;

namespace Resource.Application.Resource.Queries.GetResource
{
    public class GetResourceConsumer : IConsumer<ToolBox.Contracts.Resource.GetResource>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetResourceConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<ToolBox.Contracts.Resource.GetResource> context)
        {
            try
            {
                var resource = await _dbContext.Resources.Include(r => r.Available).Include(r => r.Reservations).FirstOrDefaultAsync(r => r.Id == context.Message.Id);
                await context.RespondAsync<ResourceVm>(new
                {
                    Resource = resource
                });
            }
            catch (Exception e)
            {
                await context.RespondAsync<NotFound>(new
                {
                    Message = e.Message
                });
            }
        }
        
    }
    public interface ResourceVm
    {
        public global::Resource.Domain.Entities.Resource Resource { get; set; }
    }
}