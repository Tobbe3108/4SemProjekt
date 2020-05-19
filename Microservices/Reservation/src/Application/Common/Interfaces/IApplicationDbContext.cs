using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Reservation> Reservations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}