using System;
using Resource.Application.Common.Interfaces;

namespace Resource.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}