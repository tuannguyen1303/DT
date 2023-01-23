using DigitalTwin.Common.Enums;
using DigitalTwin.Common.Extensions;
using DigitalTwin.Models.Requests;
using DigitalTwin.Models.Requests.Entity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalTwin.Business.Helpers
{
    public static class GetRequestFrequencyHelper
    {
        public static string GetFrequency<T>(T request) where T : class
        {
            var frequency = string.Empty;
            if (request is IFrequency)
            {
                var properties = request.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var p = property.GetValue(request)?.ToString();
                    if (property.Name == IsFrequence.IsDaily.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.Daily.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsMonthly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.Monthly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsQuarterly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.Quarterly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsMonthToDate.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.MonthToDate.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearToDaily.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.YearToDate_Daily.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearEndProjection.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.YearEndProjection.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearToMonthly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.YearToDate_Month.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsRealTime.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.RealTime.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsHour.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.Hourly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsWeekly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.Weekly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsQuarterToDate.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = Frequency.QuarterToDate.GetEnumDescription();
                    }
                }
            }
            return frequency;
        }
        public static string GetNameFrequency<T>(T request) where T : class
        {
            var frequency = string.Empty;
            if (request is IFrequency)
            {
                var properties = request.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var p = property.GetValue(request)?.ToString();
                    if (property.Name == IsFrequence.IsDaily.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsDaily.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsMonthly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsMonthly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsQuarterly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsQuarterly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsMonthToDate.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsMonthToDate.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearToDaily.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsYearToDaily.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearEndProjection.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsYearEndProjection.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsYearToMonthly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsYearToMonthly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsRealTime.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsRealTime.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsHour.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsHour.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsWeekly.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsWeekly.GetEnumDescription();
                    }
                    else if (property.Name == IsFrequence.IsQuarterToDate.ToString() && property.GetValue(request)?.ToString() == "True")
                    {
                        frequency = IsFrequence.IsQuarterToDate.GetEnumDescription();
                    }
                }
            }
            return frequency;
        }
    }
}
