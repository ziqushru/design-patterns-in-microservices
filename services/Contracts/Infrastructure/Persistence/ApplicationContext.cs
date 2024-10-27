using Contracts.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
      public virtual DbSet<Contract> Contracts =>
        Set<Contract>();
        
    public virtual DbSet<Consumer> Consumers =>
        Set<Consumer>();
        
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
