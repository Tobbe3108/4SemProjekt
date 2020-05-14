using System;
using System.Collections.Generic;
using Resource.Domain.Entities;

namespace Contracts.Resource
{
    public interface ResourceCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }
}