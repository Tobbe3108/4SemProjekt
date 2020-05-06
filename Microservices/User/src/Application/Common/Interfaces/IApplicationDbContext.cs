using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace User.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}