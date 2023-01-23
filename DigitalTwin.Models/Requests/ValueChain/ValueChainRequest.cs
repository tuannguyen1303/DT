using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;

namespace DigitalTwin.Models.Requests.ValueChain
{
    public class ValueChainRequest : BaseRequest, IRequest<Response<ValueChainResponse>>, IFrequency
    {
        public Guid ValueChainTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsRealTime { get;set; }
        public bool IsHour { get; set; }
        public bool IsDaily { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsMonthToDate { get; set; }
        public bool IsQuarterly { get; set; }
        public bool IsYearToDaily { get; set; }
        public bool IsYearToMonthly { get; set; }
        public bool IsYearEndProjection { get; set; }
        public bool IsFilter { get; set; }
        public List<Guid>? Entities { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsQuarterToDate { get; set; }
        public bool IsBusiness { get; set; }
    }
}
