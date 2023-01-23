using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.ValueChain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Requests.ValueChain
{
    public class ViewBusinessDashboardRequest : BaseRequest, IRequest<Response<ValueChainDetailResponse>>
    {
        public Guid ValueChainTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsRealTime { get; set; }
        public bool IsHour { get; set; }
        public bool IsDaily { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsMonthToDate { get; set; }
        public bool IsQuarterly { get; set; }
        public bool IsYearToDaily { get; set; }
        public bool IsYearToMonthly { get; set; }
        public bool IsYearEndProjection { get; set; }
        public bool IsFilter { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsQuarterToDate { get; set; }
        public Guid EntityId { get; set; }
        public int? TimeZoneOffset { get; set; }
        public bool IsBusiness { get; set; }
    }
}
