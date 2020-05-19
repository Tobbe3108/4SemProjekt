using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Resource.Application.Common.Interfaces;
using Resource.Domain.Common;
using Resource.Domain.Entities;
using ToolBox.Contracts.Resource;

namespace Resource.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService,
            IDateTime dateTime)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }
        
        public DbSet<Domain.Entities.Resource> Resources { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<DayAndTime> DayAndTimes { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.Username;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.Username;
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }
            
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}