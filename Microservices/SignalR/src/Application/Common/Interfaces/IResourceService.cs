using System.Threading.Tasks;

namespace SignalR.Application.Common.Interfaces
{
    public interface IResourceService
    {
        Task SendResource(Domain.Entities.Resource resource);
    }
}