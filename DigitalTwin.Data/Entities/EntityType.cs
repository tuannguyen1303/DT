namespace DigitalTwin.Data.Entities;

public class EntityType : BaseEntity
{
    public int EntityId { get; set; }
    public string? FullName { get; set; }
    public string? EntityGroupName { get; set; }
}