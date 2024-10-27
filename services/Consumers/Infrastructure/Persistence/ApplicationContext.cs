using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public virtual DbSet<Order> Orders =>
        Set<Order>();

    public virtual DbSet<OrderItem> OrderItems =>
        Set<OrderItem>();

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
