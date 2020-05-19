using System;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ToolBox.Contracts.Reservation
{
    public interface UpdateReservation
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}