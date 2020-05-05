using Resource.Application.Common.Interfaces;
using System;

namespace Resource.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}