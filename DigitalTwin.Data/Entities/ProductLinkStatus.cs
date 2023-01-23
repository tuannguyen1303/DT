namespace DigitalTwin.Data.Entities;

public class ProductLinkStatus : BaseEntity
{
    /// <summary>
    /// Product link's, Ex: 01 - green, 02 - yellow, 03 - red
    /// </summary>
    public string? NorCode { get; set; }
    
    /// <summary>
    /// Product link's color, Ex: green, yellow, red
    /// </summary>
    public string? Color { get; set; }
}