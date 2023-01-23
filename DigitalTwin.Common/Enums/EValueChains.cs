using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Common.Enums
{
    public enum Frequency
    {
        [Description("quarterly")]
        Quarterly,
        [Description("yep")]
        YearEndProjection,
        [Description("ytd")]
        YearToDate_Month,
        [Description("daily")]
        Daily,
        [Description("monthly")]
        Monthly,
        [Description("ytd_daily")]
        YearToDate_Daily,
        [Description("mtd")]
        MonthToDate,
        [Description("15m")]
        RealTime,
        [Description("hourly")]
        Hourly,
        [Description("weekly")]
        Weekly,
        [Description("qtd")]
        QuarterToDate,
    }

    public enum IsFrequence
    {
        [Description("Daily")]
        IsDaily,
        [Description("Monthly")]
        IsMonthly,
        [Description("Year End Projection")]
        IsYearEndProjection,
        [Description("Year to Date (Daily)")]
        IsYearToDaily,
        [Description("Year to Date (Month)")]
        IsYearToMonthly,
        [Description("Month To Date (MTD)")]
        IsMonthToDate,
        [Description("Every 15 mins")]
        IsRealTime,
        [Description("Hourly")]
        IsHour,
        [Description("Quarterly")]
        IsQuarterly,
        [Description("Weekly")]
        IsWeekly,
        [Description("Quarter To Date")]
        IsQuarterToDate
    }

    public enum EntityStatus
    {
        Running = 2,
        SlowDown = 3,
        ShutDown = 4
    }

    public enum Session
    {
        Exploration_Extraction,
        Processing,
        Delivery
    }
}
