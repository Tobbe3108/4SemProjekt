using System;
using SignalR.Application.Common.Interfaces;

namespace SignalR.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}