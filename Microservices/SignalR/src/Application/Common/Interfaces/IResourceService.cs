using System;
using System.Threading.Tasks;
using Type = SignalR.Domain.Enums.Type;

namespace SignalR.Application.Common.Interfaces
{
    public interface IResourceService
    {
        Task SendResource(Type type, Domain.Entities.Resource resource);
    }
}