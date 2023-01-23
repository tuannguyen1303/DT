using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.Chart;

namespace DigitalTwin.Models.Responses.ValueChain
{
    public class ValueChainDetailResponse
    {
        public ChartData? ChartData { get; set; }
        public List<ValueChainDetailDto>? PerformanceAnalysis { get; set; }
        public List<ProductionVolume>? ProductionVolume { get; set; }
        public List<ProductionVolume>? FeedstockSupply { get; set; }
        public List<ProductionVolume>? Throughput { get; set; }
        public List<ProductionVolume>? SalesVolume { get; set; }
        public List<ProductDto>? ChainOverview { get; set; }
        public DataValueChainDetail? Data { get; set; }
        public bool IsShowChart { get; set; }
        public bool IsNonFrequency { get; set; }
        public bool IsTarget { get; set; }
    }

    public class ValueChainDetailDto
    {
        public string? Actual { get; set; }
        public string? DataDate { get; set; }
        public string? Date { get; set; }
        public string? Forecast { get; set; }
        public int FunctionId { get; set; }
        public Guid Id { get; set; }    
        public string? Kpbi { get; set; }
        public string? KpbiVariance { get; set; }
        public string? Limit { get; set; }
        public string? OfftakeActual { get; set; }
        public string? OfftakeKpbi { get; set; }
        public string? OfftakeVariance { get; set; }

        public string? Planned { get; set; }
        public string? Target { get; set; }
        public string? Title { get; set;}
        public string? Unit { get; set; }
        public string? Variance { get; set; }
        public string? VariancePercentage { get; set; }
    }
    public class DataValueChainDetail
    {
        public decimal? Actual { get; set; }
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Target { get; set; }
        public string? Unit { get; set; }
        public decimal? Variance { get; set; }
        public bool IsEntityExist => Id != Guid.Empty;
        public decimal? Kpbi { get; set; }
        public string? Status { get; set; }
        public List<Justification>? Justifications { get; set; }
        public int Index { get; set; }
        public decimal? VariancePercentage { get; set; }
        public string? TitleChart { get; set; }
        public string? Alias { get; set; }
        public string? FullName { get; set; }
        public string? TargetLabel
        {
            get
            {
                return Target != null || (Target == null && Kpbi == null) ? "Planned" : "KPBI";
            }
        }
    }

    public class Justification
    {
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? Kpi_Id { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ProductionVolume
    {
        public string? Name { get; set; }
        public decimal? Actual { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Variance { get; set; }
        public decimal? Kpbi { get; set; }
        public Guid Id { get; set; }
        public string? Unit { get; set; }
        public string? Alias { get; set; }
        public string? Color { get; set; }
        public Guid? EntityId { get; set; }
    }
}
