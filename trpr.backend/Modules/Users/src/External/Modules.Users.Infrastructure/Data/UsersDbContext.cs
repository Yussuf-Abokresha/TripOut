using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Modules.Users.Domain.Entities;
using Modules.Users.Application.Abstractions;
using Common.Application;
using Common.Infrastructure.Inbox;
using Common.Infrastructure.Outbox;
using Modules.Users.Domain.Abstractions;

namespace Modules.Users.Infrastructure.Data;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork, IUsersDbContext
{
    public virtual DbSet<Token> Tokens { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Interest> Interests { get; set; }
    public virtual DbSet<Vibe> Vibes { get; set; }
    public virtual DbSet<ProfileLanguage> ProfileLanguages { get; set; }
    public virtual DbSet<ProfileInterest> ProfileInterests { get; set; }
    public virtual DbSet<ProfileVibe> ProfileVibes { get; set; }
    private IDbContextTransaction? _transaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Users);
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
