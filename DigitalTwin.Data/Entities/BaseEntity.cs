namespace DigitalTwin.Data.Entities;

public class BaseEntity
{
    /// <summary>
    /// Base Id field for each entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Base Name field for each entity
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Time of record has been created
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Time of record has been updated
    /// </summary>
    public DateTime UpdatedTime { get; set; }

    /// <summary>
    /// User's id created the record
    /// </summary>
    public ulong CreatedBy { get; set; }

    /// <summary>
    /// User's id updated the record
    /// </summary>
    public ulong UpdatedBy { get; set; }
}