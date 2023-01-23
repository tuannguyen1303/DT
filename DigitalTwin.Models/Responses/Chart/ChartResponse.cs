namespace DigitalTwin.Models.Responses.Chart
{
    public class ChartResponse
    {
        public List<ChartData>? ChartDatas { get; set; }
    }

    public class ProductChart
    {
        public Guid Id { get; set; }
        public Guid EntityMapId { get; set; }
        public string? Alias { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? NumberValues { get; set; }
        public string? Color { get; set; }
        public string? NorColor { get; set; }
        public DateTime DateData { get; set; }
    }

    public class ChartData
    {
        public List<string>? Actual { get; set; } = new();
        public List<string>? Target { get; set; } = new();
        public List<string>? Planned { get; set; } = new();
        public List<string>? Labels { get; set; } = new();
        public List<string>? ForecastBarChart { get; set; } = new();
        public List<string>? ForecastActual { get; set; } = new();
        public List<string>? CurrentForecast { get; set; } = new();
        public int CurrentIndex { get; set; }
        public int CurrentIndexAnalysis { get; set; }
        public List<string>? Kpbi { get; set; } = new();
        public bool? IsShowChart { get; set; }
        public string? Title { get; set; }
    }
}