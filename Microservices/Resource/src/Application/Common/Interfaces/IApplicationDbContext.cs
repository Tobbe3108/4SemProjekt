using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Resource.Domain.Entities;

namespace Resource.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Resource> Resources { get; set; }
        DbSet<DayAndTime> DayAndTimes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}