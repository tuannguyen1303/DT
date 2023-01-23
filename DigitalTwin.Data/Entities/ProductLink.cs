namespace DigitalTwin.Data.Entities;

public class ProductLink : BaseEntity
{
    /// <summary>
    /// Product's Unit of measure, Ex: kMT t/d, MMscfd
    /// </summary>
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public Guid? UnitOfMeasureId { get; set; }

    public string? FullName { get; set; }

    //public int NorCode { get; set; }

    /// <summary>
    /// Entity's id to destination
    /// </summary>
    public Guid? EntityMapId { get; set; }

    public Entity? Entity { get; set; }

    /// <summary>
    /// Display actual values for display on canvas view
    /// </summary>
    public decimal? Value { get; set; }

    /// <summary>
    /// Display percentage
    /// </summary>
    public decimal? Percentage { get; set; }

    /// <summary>
    /// Display variance
    /// </summary>
    public decimal? Variance { get; set; }
    public int LevelId { get; set; }
    public string? LevelName { get; set; }
}