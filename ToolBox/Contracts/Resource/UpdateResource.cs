using System;
using System.Collections.Generic;

namespace ToolBox.Contracts.Resource
{
    public interface UpdateResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
    }
}