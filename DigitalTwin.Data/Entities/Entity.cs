namespace DigitalTwin.Data.Entities;

public class Entity : BaseEntity
{
    public Guid EntityId { get; set; }

    /// <summary>
    /// Entity's value chain type, such as Gas & Oil
    /// </summary>
    public ValueChainType? ValueChainType { get; set; }

    public Guid? ValueChainTypeId { get; set; }

    /// <summary>
    /// Entity's type, such as Platform, Terminal, Customer
    /// </summary>
    public EntityType? EntityType { get; set; }
    public Guid? EntityTypeId { get; set; }

    public int EntityTypeMasterId { get; set; }

    /// <summary>
    /// Kpi path using for searching from UI, Ex: Downstream > PCCB
    /// </summary>
    public string? KpiPath { get; set; }

    /// <summary>
    /// Entity's position from root node
    /// </summary>
    public int Depth { get; set; }

    public Guid? EntityParentId { get; set; }
    public Entity? EntityParent { get; set; }
    public int LevelId { get; set; }
    public string? Density { get; set; }
    public string? RootName { get; set; }
    public int RootId { get; set; }
}