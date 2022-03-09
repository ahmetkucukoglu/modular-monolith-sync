using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Inventory.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    public DbSet<Core.Aggregates.Inventory> Inventories { get; set; }
    
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Aggregates.Inventory>().Property(x => x.Id)
            .HasConversion(x => x.Value, id => new (id));
        
        modelBuilder.Entity<Core.Aggregates.Inventory>().Property(x => x.Sku)
            .HasConversion(x => x.Sku, id => new (id));
    }
}