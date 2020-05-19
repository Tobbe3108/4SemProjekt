using System;
using Reservation.Application.Common.Interfaces;

namespace Reservation.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}