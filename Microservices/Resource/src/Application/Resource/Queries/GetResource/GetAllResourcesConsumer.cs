using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Resource.Application.Common.Interfaces;
using ToolBox.Contracts;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Resource.Queries.GetResource
{
    public class GetAllResourcesConsumer : IConsumer<GetAllResources>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetAllResourcesConsumer(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task Consume(ConsumeContext<GetAllResources> context)
        {
            try
            {
                var resources = await _dbContext.Resources.Include(r => r.Available).Include(r => r.Reservations).ToListAsync();
                await context.RespondAsync<ResourcesVm>(new
                {
                    Resources = resources
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

    public interface ResourcesVm
    {
        public IList<global::Resource.Domain.Entities.Resource> Resources { get; set; }
    }
}