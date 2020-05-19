using System;

namespace Reservation.Application.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}