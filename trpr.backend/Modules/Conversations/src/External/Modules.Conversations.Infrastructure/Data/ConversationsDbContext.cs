using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Common.Application;
using Modules.Conversations.Application.Abstractions;
using Modules.Conversations.Domain.Entities;
using Common.Infrastructure.Outbox;
using Common.Infrastructure.Inbox;

namespace Modules.Conversations.Infrastructure.Data;

public class ConversationsDbContext(DbContextOptions<ConversationsDbContext> options) : DbContext(options), IUnitOfWork, IConversationsDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageAttachment> MessageAttachments { get; set; }
    public DbSet<AiConversation> AiConversations { get; set; }
    public DbSet<AiMessage> AiMessages { get; set; }
    private IDbContextTransaction? _transaction;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Conversations);
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
}
