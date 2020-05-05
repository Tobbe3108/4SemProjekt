using System;
using Resource.Application.Common.Mappings;

namespace Resource.Application.Resource.Queries.GetResources
{
    public class ResourceDto : IMapFrom<Domain.Entities.Resource>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}