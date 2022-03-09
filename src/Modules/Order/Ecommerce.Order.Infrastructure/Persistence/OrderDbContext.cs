using Ecommerce.Order.Core.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Order.Infrastructure.Persistence;

public class OrderDbContext : DbContext
{
    public DbSet<Core.Aggregates.Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Aggregates.Order>().Property(x => x.Id)
            .HasConversion(x => x.Value, id => new (id));
        
        modelBuilder.Entity<OrderItem>().Property(x => x.Id)
            .HasConversion(x => x.Value, id => new (id));
        
        modelBuilder.Entity<OrderItem>().OwnsOne(x => x.Price,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Amount)
                    .HasColumnName("Amount");
                navigationBuilder.Property(address => address.Currency)
                    .HasColumnName("Currency");
            });
    }
}