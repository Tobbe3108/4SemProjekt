using System;
using System.Collections.Generic;
using System.Linq;
using Resource.Domain.Common;
using ToolBox.Contracts.Resource;

namespace Resource.Domain.Entities
{
    public class Resource : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DayAndTime> Available { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}