using Providers.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Providers.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public virtual DbSet<Provider> Providers =>
        Set<Provider>();

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
