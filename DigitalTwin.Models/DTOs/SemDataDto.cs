namespace DigitalTwin.Models.DTOs;

public class SemDataDto
{
    public Guid? Id { get; set; }
    public Guid? SemDataId { get; set; }
    public Guid? PathStreamId { get; set; }
    public Guid? KpiId { get; set; }
    public DateTime DataDate { get; set; }
    public string? UomName { get; set; }
    public double? ActualNum { get; set; }
    public double? TargetNum { get; set; }
    public double? VariancePercentage { get; set; }
    public string? Justification { get; set; }
    public string? Name { get; set; }
    public string? EntityName { get; set; }
    public DateTime? Updated_At { get; set; }
    public Guid? EntityParentId { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class NameDataDto
{
    public string? DisplayName { get; set; }
}