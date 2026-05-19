using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.Notifications.Application.Abstractions;
using Common.Application;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.Abstractions;
using Common.Infrastructure.Outbox;
using Common.Infrastructure.Inbox;


namespace Modules.Notifications.Infrastructure.Data;

public class NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : DbContext(options), IUnitOfWork, INotificationsDbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Template> Templates { get; set; }
    public virtual DbSet<TemplateLang> TemplateLangs { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    private IDbContextTransaction? _transaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Notifications);
        modelBuilder.ApplyConfigurationsFromAssembly
        (Modules.Notifications.Infrastructure.AssemblyRefrence.Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxConsumerMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxConsumerMessageConfiguration());
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return;

        _transaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No active transaction to commit.");

        await SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    // Async version
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        // This tracks all changed entries and updates the timestamps
        var entries = ChangeTracker.Entries<Entity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAtUTC = DateTime.UtcNow;
                entity.UpdatedAtUTC = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAtUTC = DateTime.UtcNow;
            }
        }
    }
}
