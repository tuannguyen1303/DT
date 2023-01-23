namespace DigitalTwin.Data.Entities;

public class ValueChainType : BaseEntity
{
    /// <summary>
    /// Type 1: Gas; Type 2: Oil
    /// </summary>
    public ChainType Type { get; set; }
}

public enum ChainType
{
    Gas = 1,
    Oil = 2
}