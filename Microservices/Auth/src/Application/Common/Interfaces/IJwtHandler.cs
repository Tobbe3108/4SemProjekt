using System;
using Auth.Application.Common.Models;

namespace Auth.Application.Common.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken Crete(Guid userId);
    }
}