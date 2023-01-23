using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Chart;
using MediatR;

namespace DigitalTwin.Models.Requests.Chart
{
    public class ChartRequest : BaseRequest, IRequest<Response<ChartResponse>>, IFrequency
    {
        public List<Guid>? ProductLinkId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsRealTime { get; set; }
        public bool IsHour { get; set; }
        public bool IsDaily { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsMonthToDate { get; set; }
        public bool IsQuarterly { get; set; }
        public bool IsYearToDaily { get; set; }
        public bool IsYearToMonthly { get; set; }
        public bool IsYearEndProjection { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsQuarterToDate { get; set; }
        public int? TimeZoneOffset { get; set; }
        public bool IsBusiness { get; set; }
    }
}