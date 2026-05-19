using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Common.Application;
using Modules.Trips.Application.Abstractions;
using Common.Infrastructure.Outbox;
using Common.Infrastructure.Inbox;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Domain.Abstractions;

namespace Modules.Trips.Infrastructure.Data;

public class TripsDbContext(DbContextOptions<TripsDbContext> options) : DbContext(options), IUnitOfWork, ITripsDbContext
{
    private IDbContextTransaction? _transaction;
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Governorate> Governorates { get; set; }
    public virtual DbSet<Place> Places { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<PlaceTag> PlaceTags { get; set; }
    public virtual DbSet<Theme> Themes { get; set; }
    public virtual DbSet<ThemeTag> ThemeTags { get; set; }
    public virtual DbSet<ThemeCategory> ThemeCategories { get; set; }
    public virtual DbSet<CsvSeedHistory> CsvSeedHistories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Trips);
        modelBuilder.ApplyConfigurationsFromAssembly
        (AssemblyRefrence.Assembly);

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
