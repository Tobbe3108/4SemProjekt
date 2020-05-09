using System;
using System.Collections.Generic;
using User.Application.Common.Interfaces;
using User.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToolBox.Events;
using User.Domain.Delegates;
using User.Domain.Entities;

namespace User.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IEnumerable<IEventMapper> _eventMappers;
        private IDbContextTransaction _currentTransaction;
        private readonly OnNewOutboxMessages _onNewOutboxMessages;

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IEnumerable<IEventMapper> eventMappers,
            OnNewOutboxMessages onNewOutboxMessages) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _eventMappers = eventMappers;
            _onNewOutboxMessages = onNewOutboxMessages;
        }

        public DbSet<Domain.Entities.User> Users { get; set; }

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

            var eventsDetected = GetEvents();
            AddEventIfAny(eventsDetected);

            var result = await base.SaveChangesAsync(cancellationToken);

            NotifyEventsIfAny(eventsDetected);
            
            return result;
        }

        #region SaveChangesAsync Helper Methods
        private IReadOnlyCollection<OutboxMessage> GetEvents()
        {
            var now = _dateTime.Now;

            return _eventMappers
                .SelectMany(mapper => mapper.Map(this))
                .ToList();
        }

        private void AddEventIfAny(IReadOnlyCollection<OutboxMessage> collection)
        {
            if (collection.Count > 0)
            {
                Set<OutboxMessage>().AddRange(collection);
            }
        }

        private void NotifyEventsIfAny(IReadOnlyCollection<OutboxMessage> eventsDetected)
        {
            if (eventsDetected.Count > 0)
            {
                _onNewOutboxMessages(eventsDetected.Select(e => e.Id));
            }
        }
        #endregion

        #region Transaction
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted)
                .ConfigureAwait(false);
        }
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}