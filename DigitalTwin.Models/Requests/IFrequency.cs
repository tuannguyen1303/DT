using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Models.Requests
{
    public interface IFrequency
    {
        public bool IsDaily { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsMonthToDate { get; set; }
        public bool IsQuarterly { get; set; }
        public bool IsYearToDaily { get; set; }
        public bool IsYearToMonthly { get; set; }
        public bool IsYearEndProjection { get; set; }
        public bool IsRealTime { get; set; }
        public bool IsHour { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsQuarterToDate { get; set; }
    }
}
