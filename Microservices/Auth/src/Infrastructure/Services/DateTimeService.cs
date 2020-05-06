using System;
using Auth.Application.Common.Interfaces;

namespace Auth.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}