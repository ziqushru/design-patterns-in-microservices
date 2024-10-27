using Consumers.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consumers.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public virtual DbSet<Consumer> Consumers =>
        Set<Consumer>();

    public ApplicationContext()
    { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
          : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCharSet("utf8mb4");

        base.OnModelCreating(modelBuilder);
    }
}
