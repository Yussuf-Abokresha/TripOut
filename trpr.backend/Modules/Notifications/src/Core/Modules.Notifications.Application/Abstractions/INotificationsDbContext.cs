using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Domain.Entities;

namespace Modules.Notifications.Application.Abstractions;

public interface INotificationsDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Template> Templates { get; set; }
}
