using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using User.Domain.Entities;

namespace User.Application.Common.Interfaces
{
    public interface IEventMapper
    {
        IEnumerable<OutboxMessage> Map(DbContext dbContext);
    }
}