using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToolBox.Contracts.Resource;

namespace Resource.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Resource> Resources { get; set; }
        DbSet<Domain.Entities.Reservation> Reservations { get; set; }
        DbSet<DayAndTime> DayAndTimes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        EntityEntry Entry(object entity);
    }
}