using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Resource.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Resource> Resources { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}