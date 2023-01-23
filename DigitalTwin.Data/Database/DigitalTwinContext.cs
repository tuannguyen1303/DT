using DigitalTwin.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalTwin.Data.Database;

public class DigitalTwinContext : DbContext
{
    public DigitalTwinContext(DbContextOptions<DigitalTwinContext> options) : base(options)
    {
    }

    public DigitalTwinContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("pivot_da_middleware_digital");
    }

    public DbSet<ValueChainType>? ValueChainTypes { get; set; }
    public DbSet<Entity>? Entities { get; set; }
    public DbSet<EntityType>? EntityTypes { get; set; }
    public DbSet<ProductLink>? ProductLinks { get; set; }
    public DbSet<ProductLinkStatus>? ProductLinkStatuses { get; set; }
    public DbSet<UnitOfMeasure>? UnitOfMeasures { get; set; }
    public DbSet<ProductLinkDetail>? ProductLinkDetails { get; set; }
}