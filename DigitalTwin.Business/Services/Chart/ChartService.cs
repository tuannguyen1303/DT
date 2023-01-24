using System.Linq.Expressions;
using DigitalTwin.Business.Extensions;
using DigitalTwin.Business.Helpers;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Extensions;
using DigitalTwin.Data.Database;
using DigitalTwin.Models.Requests.Chart;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Chart;
using DigitalTwin.Models.Responses.ValueChain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalTwin.Business.Services.Chart
{
    public class ChartService : IChartService
    {
        private readonly DigitalTwinContext _context;

        public ChartService(DigitalTwinContext context)
        {
            _context = context;
        }

        public ChartData GetChart(ChartRequest request, CancellationToken token)
        {
            var chartData = new ChartData();
            chartData.IsShowChart = false;
            chartData.CurrentIndex = 0;
            var frequency = GetRequestFrequencyHelper.GetFrequency(request);
            if (request.IsDaily || request.IsMonthToDate)
            {
                chartData.CurrentIndex = request.ToDate!.Value.Day - 1;
                chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                request.FromDate = new DateTime(request.ToDate!.Value.Year, request.ToDate!.Value.Month, 1, 0, 0, 0)
                    .ToUTC(TimeSpan.Zero).AddDays(-1);
                request.ToDate =
                    new DateTime(request.ToDate!.Value.Year, request.ToDate!.Value.Month + 1, 1, 23, 59, 59).AddDays(-1)
                        .ToUTC(TimeSpan.Zero);
                var productLinkDto = GetProductDto(request, frequency);
                var daysInMonth = FrequencyLabel.DailyLabels(request.ToDate!.Value.Month, request.ToDate.Value.Year);
                bool checkForecast = true;
                foreach (var label in daysInMonth)
                {
                    var List = productLinkDto.Where(p => p.DateData.Day == 1).ToList();
                    bool checkHasData = false;

                    // Expression<Func<ProductDetailDto, bool>> mainExpression = dto =>
                    //     dto.DateData.Day == label && dto.DateData.Month == request.ToDate.Value.Month;
                    //
                    // var kpbiTMP = productLinkDto.SumDecimalByProp(
                    //     LinqQueryExtension.AndQuery(mainExpression, x => !string.IsNullOrEmpty(x.Kbpi)),
                    //     x => decimal.Parse(x.Kbpi!));

                    decimal? kpbi = productLinkDto.Any(p =>
                        p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                        !string.IsNullOrEmpty(p.Kbpi))
                        ? productLinkDto.Where(p =>
                                p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                                !string.IsNullOrEmpty(p.Kbpi))
                            .Sum(p => decimal.Parse(p.Kbpi!))
                        : null;

                    decimal? actual = productLinkDto.Any(p =>
                        p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                        !string.IsNullOrEmpty(p.Value))
                        ? productLinkDto.Where(p =>
                                p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                                !string.IsNullOrEmpty(p.Value))
                            .Sum(p => decimal.Parse(p.Value!))
                        : null;

                    decimal? target = productLinkDto.Any(p =>
                        p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                        !string.IsNullOrEmpty(p.Target))
                        ? productLinkDto.Where(p =>
                                p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                                !string.IsNullOrEmpty(p.Target))
                            .Sum(p => decimal.Parse(p.Target!))
                        : null;
                    decimal? planned = productLinkDto.Any(p =>
                        p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                        !string.IsNullOrEmpty(p.Planned))
                        ? productLinkDto.Where(p =>
                                p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                                !string.IsNullOrEmpty(p.Planned))
                            .Sum(p => decimal.Parse(p.Planned!))
                        : null;
                    decimal? forecast = productLinkDto.Any(p =>
                        p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                        !string.IsNullOrEmpty(p.Forecast))
                        ? productLinkDto.Where(p =>
                                p.DateData.Day == label && p.DateData.Month == request.ToDate.Value.Month &&
                                !string.IsNullOrEmpty(p.Forecast))
                            .Sum(p => decimal.Parse(p.Forecast!))
                        : null;
                    string title = productLinkDto.Any()
                        ? productLinkDto.FirstOrDefault(p => !string.IsNullOrEmpty(p.Unit))!.Unit!
                        : "";
                    if (kpbi != 0 || actual != 0 || target != 0 || planned != 0)
                    {
                        checkHasData = true;
                        chartData!.Title = title;
                        chartData.Labels?.Add(label.ToString());
                        chartData?.Kpbi?.Add(kpbi != null ? kpbi.ToString()! : null!);
                        chartData?.Target?.Add(target != null ? target.ToString()! : null!);
                        chartData?.Planned?.Add(planned != null ? planned.ToString()! : null!);
                        chartData?.ForecastBarChart?.Add(forecast != null ? forecast.ToString()! : null!);
                        if (actual != null)
                        {
                            chartData?.ForecastActual?.Add(null!);
                        }
                        else
                        {
                            int forecastIndex = chartData!.Actual!.Count();
                            if (checkForecast && forecastIndex > 0 && forecast != null)
                            {
                                chartData.ForecastActual![forecastIndex - 1] = chartData.Actual![forecastIndex - 1];
                                checkForecast = false;
                            }

                            chartData?.ForecastActual?.Add(forecast != null ? forecast.ToString()! : null!);
                        }

                        chartData?.Actual?.Add(actual != null ? actual.ToString()! : null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }

                    if (checkHasData == false)
                    {
                        chartData!.Labels?.Add(label.ToString());
                        chartData?.Kpbi?.Add(null!);
                        chartData?.Actual?.Add(null!);
                        chartData?.Target?.Add(null!);
                        chartData?.Planned?.Add(null!);
                        chartData?.ForecastBarChart?.Add(null!);
                        chartData?.ForecastActual?.Add(null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }
                }
            }
            else if (request.IsMonthly || request.IsYearToDaily || request.IsYearEndProjection ||
                     request.IsYearToMonthly)
            {
                chartData.CurrentIndex = request.ToDate!.Value.Month - 1;
                chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                request.FromDate = new DateTime(request.ToDate!.Value.Year, 1, 1, 0, 0, 0).ToUTC(TimeSpan.Zero)
                    .AddDays(-1);
                request.ToDate = new DateTime(request.ToDate.Value.Year, 12, 31, 23, 59, 59).ToUTC(TimeSpan.Zero);
                var productLinkDto = GetProductDto(request, frequency);

                bool checkForecast = true;
                foreach (var label in FrequencyLabel.MonthlyLabels)
                {
                    bool checkHasData = false;

                    var mainQuery = LinqQueryExtension.BuildQuery<ProductDetailDto>(true,
                        p => p.DateData.Month == label.MonthNumb,
                        p => p.DateData.Year == request.ToDate.Value.Year);

                    var kpbiTMP = productLinkDto.SumDecimalByProp(
                        LinqQueryExtension.CombineAndQuery(mainQuery!, x => !string.IsNullOrEmpty(x.Kbpi)),
                        x => decimal.Parse(x.Kbpi!));

                    decimal? kpbi = productLinkDto.Any(p =>
                        p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                        !string.IsNullOrEmpty(p.Kbpi))
                        ? productLinkDto.Where(p =>
                                p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                                !string.IsNullOrEmpty(p.Kbpi))
                            .Sum(p => decimal.Parse(p.Kbpi!))
                        : null;

                    var actualTMP = productLinkDto.SumDecimalByProp(
                        LinqQueryExtension.CombineAndQuery(mainQuery!, x => !string.IsNullOrEmpty(x.Value)),
                        x => decimal.Parse(x.Value!));

                    decimal? actual = productLinkDto.Any(p =>
                        p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                        !string.IsNullOrEmpty(p.Value))
                        ? productLinkDto.Where(p =>
                                p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                                !string.IsNullOrEmpty(p.Value))
                            .Sum(p => decimal.Parse(p.Value!))
                        : null;

                    decimal? target = productLinkDto.Any(p =>
                        p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                        !string.IsNullOrEmpty(p.Target))
                        ? productLinkDto.Where(p =>
                                p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                                !string.IsNullOrEmpty(p.Target))
                            .Sum(p => decimal.Parse(p.Target!))
                        : null;

                    decimal? planned = productLinkDto.Any(p =>
                        p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                        !string.IsNullOrEmpty(p.Planned))
                        ? productLinkDto.Where(p =>
                                p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                                !string.IsNullOrEmpty(p.Planned))
                            .Sum(p => decimal.Parse(p.Planned!))
                        : null;

                    decimal? forecast = productLinkDto.Any(p =>
                        p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                        !string.IsNullOrEmpty(p.Forecast))
                        ? productLinkDto.Where(p =>
                                p.DateData.Month == label.MonthNumb && p.DateData.Year == request.ToDate.Value.Year &&
                                !string.IsNullOrEmpty(p.Forecast))
                            .Sum(p => decimal.Parse(p.Forecast!))
                        : null;

                    string title = productLinkDto.Any()
                        ? productLinkDto.FirstOrDefault(p => !string.IsNullOrEmpty(p.Unit))!.Unit!
                        : "";

                    if (kpbi != 0 || actual != 0 || target != 0 || planned != 0 || forecast != 0)
                    {
                        checkHasData = true;
                        chartData!.Title = title;
                        chartData.Labels?.Add(label.Month! + " " + request.ToDate!.Value.Year);
                        chartData?.Kpbi?.Add(kpbi != null ? kpbi.ToString()! : null!);
                        chartData?.Target?.Add(target != null ? target.ToString()! : null!);
                        chartData?.Planned?.Add(planned != null ? planned.ToString()! : null!);
                        chartData?.ForecastBarChart?.Add(forecast != null ? forecast.ToString()! : null!);
                        if (actual != null)
                        {
                            chartData?.ForecastActual?.Add(null!);
                        }
                        else
                        {
                            int forecastIndex = chartData!.Actual!.Count();
                            if (checkForecast && forecastIndex > 0 && forecast != null)
                            {
                                chartData.ForecastActual![forecastIndex - 1] = chartData.Actual![forecastIndex - 1];
                                checkForecast = false;
                            }

                            chartData?.ForecastActual?.Add(forecast != null ? forecast.ToString()! : null!);
                        }

                        chartData?.Actual?.Add(actual != null ? actual.ToString()! : null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }

                    if (checkHasData == false)
                    {
                        chartData!.Labels?.Add(label.Month! + " " + request.ToDate!.Value.Year);
                        chartData?.Kpbi?.Add(null!);
                        chartData?.Actual?.Add(null!);
                        chartData?.Target?.Add(null!);
                        chartData?.Planned?.Add(null!);
                        chartData?.ForecastBarChart?.Add(null!);
                        chartData?.ForecastActual?.Add(null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }
                }
            }
            else if (request.IsQuarterly)
            {
                chartData.CurrentIndex = FrequencyLabel.QuarterlyIndex(request.ToDate!.Value.Month);
                chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                request.FromDate = new DateTime(request.FromDate!.Value.Year, 1, 1, 0, 0, 0).ToUTC(TimeSpan.Zero)
                    .AddDays(-1);
                request.ToDate = new DateTime(request.ToDate.Value.Year, 12, 31, 23, 59, 59).ToUTC(TimeSpan.Zero);
                var productLinkDto = GetProductDto(request, frequency);
                productLinkDto = productLinkDto.Where(p => p.DateData.Year == request.ToDate.Value.Year)
                    .OrderBy(c => c.DateData).ToList();
                bool checkForecast = true;
                foreach (var label in FrequencyLabel.QuarterlyLabels)
                {
                    bool checkHasData = false;

                    Expression<Func<ProductDetailDto, bool>> expression = dto =>
                        FrequencyLabel.Quarterly(dto.DateData.Month) == label &&
                        dto.DateData.Year == request.ToDate.Value.Year
                        && !string.IsNullOrEmpty(dto.Kbpi);

                    decimal? kpbi = productLinkDto.Any(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                            p.DateData.Year == request.ToDate.Value.Year
                                                            && !string.IsNullOrEmpty(p.Kbpi))
                        ? productLinkDto.Where(expression.Compile())
                            .Sum(p => decimal.Parse(p.Kbpi!))
                        : null;
                    decimal? actual = productLinkDto.Any(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                              p.DateData.Year == request.ToDate.Value.Year
                                                              && !string.IsNullOrEmpty(p.Value))
                        ? productLinkDto.Where(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                    p.DateData.Year == request.ToDate.Value.Year
                                                    && !string.IsNullOrEmpty(p.Value))
                            .Sum(p => decimal.Parse(p.Value!))
                        : null;
                    decimal? target = productLinkDto.Any(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                              p.DateData.Year == request.ToDate.Value.Year
                                                              && !string.IsNullOrEmpty(p.Target))
                        ? productLinkDto.Where(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                    p.DateData.Year == request.ToDate.Value.Year
                                                    && !string.IsNullOrEmpty(p.Target))
                            .Sum(p => decimal.Parse(p.Target!))
                        : null;
                    decimal? planned = productLinkDto.Any(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                               p.DateData.Year == request.ToDate.Value.Year
                                                               && !string.IsNullOrEmpty(p.Planned))
                        ? productLinkDto.Where(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                    p.DateData.Year == request.ToDate.Value.Year
                                                    && !string.IsNullOrEmpty(p.Planned))
                            .Sum(p => decimal.Parse(p.Planned!))
                        : null;
                    decimal? forecast = productLinkDto.Any(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                                p.DateData.Year == request.ToDate.Value.Year
                                                                && !string.IsNullOrEmpty(p.Forecast))
                        ? productLinkDto.Where(p => FrequencyLabel.Quarterly(p.DateData.Month) == label &&
                                                    p.DateData.Year == request.ToDate.Value.Year
                                                    && !string.IsNullOrEmpty(p.Forecast))
                            .Sum(p => decimal.Parse(p.Forecast!))
                        : null;
                    string title = productLinkDto.Any()
                        ? productLinkDto.FirstOrDefault(p => !string.IsNullOrEmpty(p.Unit))!.Unit!
                        : "";
                    if (kpbi != 0 || actual != 0 || target != 0 || planned != 0)
                    {
                        checkHasData = true;
                        chartData!.Title = title;
                        chartData.Labels?.Add(label + "/" + request.ToDate.Value.Year);
                        chartData?.Kpbi?.Add(kpbi != null ? kpbi.ToString()! : null!);
                        chartData?.Target?.Add(target != null ? target.ToString()! : null!);
                        chartData?.Planned?.Add(planned != null ? planned.ToString()! : null!);
                        chartData?.ForecastBarChart?.Add(forecast != null ? forecast.ToString()! : null!);
                        if (actual != null)
                        {
                            chartData?.ForecastActual?.Add(null!);
                        }
                        else
                        {
                            int forecastIndex = chartData!.Actual!.Count();
                            if (checkForecast && forecastIndex > 0 && forecast != null)
                            {
                                chartData.ForecastActual![forecastIndex - 1] = chartData.Actual![forecastIndex - 1];
                                checkForecast = false;
                            }

                            chartData?.ForecastActual?.Add(forecast != null ? forecast.ToString()! : null!);
                        }

                        chartData?.Actual?.Add(actual != null ? actual.ToString()! : null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }

                    if (checkHasData == false)
                    {
                        chartData!.Labels?.Add(label + "/" + request.ToDate.Value.Year);
                        chartData?.Kpbi?.Add(null!);
                        chartData?.Actual?.Add(null!);
                        chartData?.Target?.Add(null!);
                        chartData?.Planned?.Add(null!);
                        chartData?.ForecastBarChart?.Add(null!);
                        chartData?.ForecastActual?.Add(null!);
                        chartData?.CurrentForecast?.Add(null!);
                    }
                }
            }
            else if (request.IsRealTime)
            {
                var dateRequest = new DateTime(request.ToDate!.Value.Year,
                    request.ToDate!.Value.Month,
                    request.ToDate!.Value.Day,
                    DateTime.UtcNow.Hour,
                    DateTime.UtcNow.Minute, 00).ToUTC(TimeSpan.Zero);
                if (dateRequest.Minute != 0 && dateRequest.Minute != 00 && dateRequest.Minute != 30)
                {
                    request.FromDate =
                        FrequencyLabel.RoundUpDateTime(dateRequest.AddHours(-2), TimeSpan.FromMinutes(30));
                    request.ToDate = FrequencyLabel.RoundUpDateTime(dateRequest.AddHours(2), TimeSpan.FromMinutes(30));
                    chartData.CurrentIndex = 7;
                    chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                }
                else
                {
                    request.FromDate =
                        FrequencyLabel.RoundUpDateTime(dateRequest.AddHours(-1.5), TimeSpan.FromMinutes(30));
                    request.ToDate =
                        FrequencyLabel.RoundUpDateTime(dateRequest.AddHours(-1.5), TimeSpan.FromMinutes(30));
                    chartData.CurrentIndex = 6;
                    chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                }

                var productLinkDto = GetProductDto(request, frequency);
                productLinkDto = productLinkDto.OrderBy(c => c.DateData).ToList();
                DateTime hourLabel = (DateTime)request.FromDate.Value.AddHours(request.TimeZoneOffset ?? 0);
                bool checkForecast = true;

                for (int i = 0; i < FrequencyLabel.RealTimeLabels.Count; i++)
                {
                    bool checkHasData = false;
                    decimal? kpbi = productLinkDto.Any(p =>
                        hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                        !string.IsNullOrEmpty(p.Kbpi))
                        ? productLinkDto.Where(p =>
                                hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                                !string.IsNullOrEmpty(p.Kbpi))
                            .Sum(p => decimal.Parse(p.Kbpi!))
                        : null;
                    decimal? actual = productLinkDto.Any(p =>
                        hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                        !string.IsNullOrEmpty(p.Value))
                        ? productLinkDto.Where(p =>
                                hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                                !string.IsNullOrEmpty(p.Value))
                            .Sum(p => decimal.Parse(p.Value!))
                        : null;
                    decimal? target = productLinkDto.Any(p =>
                        hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                        !string.IsNullOrEmpty(p.Target))
                        ? productLinkDto.Where(p =>
                                hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                                !string.IsNullOrEmpty(p.Target))
                            .Sum(p => decimal.Parse(p.Target!))
                        : null;
                    decimal? planned = productLinkDto.Any(p =>
                        hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                        !string.IsNullOrEmpty(p.Planned))
                        ? productLinkDto.Where(p =>
                                hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                                !string.IsNullOrEmpty(p.Planned))
                            .Sum(p => decimal.Parse(p.Planned!))
                        : null;
                    decimal? forecast = productLinkDto.Any(p =>
                        hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                        !string.IsNullOrEmpty(p.Forecast))
                        ? productLinkDto.Where(p =>
                                hourLabel.ToShortTimeString() == p.DateData.ToShortTimeString() &&
                                !string.IsNullOrEmpty(p.Forecast))
                            .Sum(p => decimal.Parse(p.Forecast!))
                        : null;
                    string title = productLinkDto.Any()
                        ? productLinkDto.FirstOrDefault(p => !string.IsNullOrEmpty(p.Unit))!.Unit!
                        : "";
                    if (kpbi != 0 || actual != 0 || target != 0 || planned != 0)
                    {
                        checkHasData = true;
                        chartData!.Title = title;
                        if (hourLabel.Minute == 0)
                            chartData.Labels?.Add(hourLabel.Hour + ":" + hourLabel.Minute + "0");
                        else
                            chartData.Labels?.Add(hourLabel.Hour + ":" + hourLabel.Minute);
                        chartData?.Kpbi?.Add(kpbi != null ? kpbi.ToString()! : null!);
                        chartData?.Target?.Add(target != null ? target.ToString()! : null!);
                        chartData?.Planned?.Add(planned != null ? planned.ToString()! : null!);
                        chartData?.ForecastBarChart?.Add(forecast != null ? forecast.ToString()! : null!);
                        if (actual != null)
                        {
                            chartData?.ForecastActual?.Add(null!);
                        }
                        else
                        {
                            int forecastIndex = chartData!.Actual!.Count();
                            if (checkForecast && forecastIndex > 0 && forecast != null)
                            {
                                chartData.ForecastActual![forecastIndex - 1] = chartData.Actual![forecastIndex - 1];
                                checkForecast = false;
                            }

                            chartData?.ForecastActual?.Add(forecast != null ? forecast.ToString()! : null!);
                        }

                        chartData?.Actual?.Add(actual != null ? actual.ToString()! : null!);
                        chartData?.CurrentForecast?.Add(null!);
                        hourLabel = hourLabel.AddMinutes(15);
                    }

                    if (checkHasData == false)
                    {
                        if (hourLabel.Minute == 0)
                            chartData!.Labels?.Add(hourLabel.Hour + ":" + hourLabel.Minute + "0");
                        else
                            chartData!.Labels?.Add(hourLabel.Hour + ":" + hourLabel.Minute);
                        chartData?.Kpbi?.Add(null!);
                        chartData?.Actual?.Add(null!);
                        chartData?.Target?.Add(null!);
                        chartData?.Planned?.Add(null!);
                        chartData?.ForecastBarChart?.Add(null!);
                        chartData?.ForecastActual?.Add(null!);
                        chartData?.CurrentForecast?.Add(null!);
                        hourLabel = hourLabel.AddMinutes(15);
                    }
                }
            }
            else if (request.IsHour)
            {
                chartData.CurrentIndex = DateTime.UtcNow.AddHours(request.TimeZoneOffset ?? 0).Hour;
                chartData.CurrentIndexAnalysis = chartData.CurrentIndex;
                request.FromDate = new DateTime(request.ToDate!.Value.Year, request.ToDate!.Value.Month,
                    request.ToDate!.Value.Day, 0, 0, 0).ToUTC(TimeSpan.Zero);
                request.ToDate = new DateTime(request.ToDate!.Value.Year, request.ToDate!.Value.Month,
                    request.ToDate!.Value.Day + 1, 0, 0, 0).ToUTC(TimeSpan.Zero);
                var productLinkDto = GetProductDto(request, frequency);
                productLinkDto = productLinkDto.OrderBy(c => c.DateData).ToList();
                DateTime hourLabel = (DateTime)request.FromDate.Value.ToUTC(TimeSpan.Zero);
                bool checkForecast = true;

                foreach (var label in FrequencyLabel.HourlyLabels)
                {
                    if (label == "24:00")
                    {
                        hourLabel = (DateTime)request.FromDate.Value.ToUTC(TimeSpan.Zero).AddDays(1);
                    }

                    bool checkHasData = false;
                    decimal? kpbi = productLinkDto.Any(p =>
                        p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                        !string.IsNullOrEmpty(p.Kbpi))
                        ? productLinkDto.Where(p =>
                                p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                                !string.IsNullOrEmpty(p.Kbpi))
                            .Sum(p => decimal.Parse(p.Kbpi!))
                        : null;
                    decimal? actual = productLinkDto.Any(p =>
                        p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                        !string.IsNullOrEmpty(p.Value))
                        ? productLinkDto.Where(p =>
                                p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                                !string.IsNullOrEmpty(p.Value))
                            .Sum(p => decimal.Parse(p.Value!))
                        : null;
                    decimal? target = productLinkDto.Any(p =>
                        p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                        !string.IsNullOrEmpty(p.Target))
                        ? productLinkDto.Where(p =>
                                p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                                !string.IsNullOrEmpty(p.Target))
                            .Sum(p => decimal.Parse(p.Target!))
                        : null;
                    decimal? planned = productLinkDto.Any(p =>
                        p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                        !string.IsNullOrEmpty(p.Planned))
                        ? productLinkDto.Where(p =>
                                p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                                !string.IsNullOrEmpty(p.Planned))
                            .Sum(p => decimal.Parse(p.Planned!))
                        : null;
                    decimal? forecast = productLinkDto.Any(p =>
                        p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                        !string.IsNullOrEmpty(p.Forecast))
                        ? productLinkDto.Where(p =>
                                p.DateData.Hour == hourLabel.Hour && p.DateData.Day == hourLabel.Day &&
                                !string.IsNullOrEmpty(p.Forecast))
                            .Sum(p => decimal.Parse(p.Forecast!))
                        : null;
                    string title = productLinkDto.Any()
                        ? productLinkDto.FirstOrDefault(p => !string.IsNullOrEmpty(p.Unit))!.Unit!
                        : "";
                    if (kpbi != 0 || actual != 0 || target != 0 || planned != 0)
                    {
                        checkHasData = true;
                        chartData!.Title = title;
                        chartData.Labels?.Add(label);
                        chartData?.Kpbi?.Add(kpbi != null ? kpbi.ToString()! : null!);
                        chartData?.Target?.Add(target != null ? target.ToString()! : null!);
                        chartData?.Planned?.Add(planned != null ? planned.ToString()! : null!);
                        chartData?.ForecastBarChart?.Add(forecast != null ? forecast.ToString()! : null!);
                        if (actual != null)
                        {
                            chartData?.ForecastActual?.Add(null!);
                        }
                        else
                        {
                            int forecastIndex = chartData!.Actual!.Count();
                            if (checkForecast && forecastIndex > 0 && forecast != null)
                            {
                                chartData.ForecastActual![forecastIndex - 1] = chartData.Actual![forecastIndex - 1];
                                checkForecast = false;
                            }

                            chartData?.ForecastActual?.Add(forecast != null ? forecast.ToString()! : null!);
                        }

                        chartData?.Actual?.Add(actual != null ? actual.ToString()! : null!);
                        chartData?.CurrentForecast?.Add(null!);
                        hourLabel = hourLabel.AddHours(1);
                    }

                    if (checkHasData == false)
                    {
                        chartData!.Labels?.Add(label);
                        chartData?.Kpbi?.Add(null!);
                        chartData?.Actual?.Add(null!);
                        chartData?.Target?.Add(null!);
                        chartData?.Planned?.Add(null!);
                        chartData?.ForecastBarChart?.Add(null!);
                        chartData?.ForecastActual?.Add(null!);
                        chartData?.CurrentForecast?.Add(null!);
                        hourLabel = hourLabel.AddHours(1);
                    }
                }
            }

            chartData!.IsShowChart = chartData.Actual!.Any(c => !string.IsNullOrEmpty(c)) ||
                                     chartData.Kpbi!.Any(c => !string.IsNullOrEmpty(c))
                                     || chartData.Target!.Any(c => !string.IsNullOrEmpty(c)) ||
                                     chartData.Planned!.Any(c => !string.IsNullOrEmpty(c));

            return chartData!;
        }

        private List<ProductDetailDto> GetProductDto(ChartRequest request, string frequency)
        {
            var productLinkDetailDtos = new List<ProductDetailDto>();
            var productNameList = new List<string>
            {
                DigitalTwinConstants.KPI_Feedstock, DigitalTwinConstants.KPI_ProductionVolume,
                DigitalTwinConstants.CustomerName
            };
            var productLinkDto = new List<ProductChart>();
            if (request.IsBusiness)
            {
                productLinkDto = (from pld in _context.ProductLinkDetails
                        join pl in _context.ProductLinks! on pld.ProductLinkId equals pl.Id
                        join uom in _context.UnitOfMeasures! on pl.UnitOfMeasureId equals uom.Id
                        join e in _context.Entities! on pl.EntityMapId equals e.EntityId
                        where pld.DataDate >= request.FromDate && pld.DataDate <= request.ToDate
                                                               && pl.EntityMapId.HasValue &&
                                                               DigitalTwinConstants.BusinessLevels.Contains(pl.LevelId)
                                                               && frequency == pld.Frequency &&
                                                               DigitalTwinConstants.BusinessLevelName.Contains(
                                                                   pl.LevelName!)
                                                               && pl.Name == DigitalTwinConstants.BusinessName
                                                               && request.ProductLinkId!.Contains(pl.Id)
                        orderby pld.DataDate
                        select new ProductChart
                        {
                            Id = pl.Id,
                            EntityMapId = pl.EntityMapId!.Value,
                            Alias = pl.Name!.ToLower(),
                            Name = pl.Name,
                            Unit = uom.Name,
                            NumberValues = pld.NumValues,
                            Color = pld.Color!.Replace("\"", string.Empty),
                            NorColor = pld.NorCode,
                            DateData = (DateTime)pld.DataDate!
                        })
                    .OrderBy(c => c.DateData)
                    .Distinct()
                    .AsNoTracking().ToList();
            }
            else
            {
                productLinkDto = (from pld in _context.ProductLinkDetails
                        join pl in _context.ProductLinks! on pld.ProductLinkId equals pl.Id
                        join uom in _context.UnitOfMeasures! on pl.UnitOfMeasureId equals uom.Id
                        join e in _context.Entities! on pl.EntityMapId equals e.EntityId
                        where pld.DataDate >= request.FromDate && pld.DataDate <= request.ToDate
                                                               && pl.EntityMapId.HasValue && pl.LevelId == 8
                                                               && frequency == pld.Frequency &&
                                                               pl.LevelName == DigitalTwinConstants.ProductCateogry
                                                               && productNameList.Contains(pl.Name!)
                                                               && request.ProductLinkId!.Contains(e.Id)
                        orderby pld.DataDate
                        select new ProductChart
                        {
                            Id = pl.Id,
                            EntityMapId = pl.EntityMapId!.Value,
                            Alias = pl.Name!.ToLower(),
                            Name = pl.Name,
                            Unit = uom.Name,
                            NumberValues = pld.NumValues,
                            Color = pld.Color!.Replace("\"", string.Empty),
                            NorColor = pld.NorCode,
                            DateData = (DateTime)pld.DataDate!
                        })
                    .OrderBy(c => c.DateData)
                    .Distinct()
                    .AsNoTracking().ToList();
            }

            productLinkDto.ForEach(c =>
            {
                dynamic data = JsonConvert.DeserializeObject(c!.NumberValues);
                productLinkDetailDtos.Add(new ProductDetailDto
                {
                    Id = c.Id,
                    EntityMapId = c.EntityMapId,
                    Name = c.Name != null ? c.Name.ToString() : string.Empty,
                    Unit = c.Unit != null ? c.Unit.ToString() : string.Empty,
                    Color = c.Color != null ? c.Color.ToString().Replace("\"", string.Empty) : string.Empty,
                    DateData = c.DateData.AddHours(request.TimeZoneOffset.HasValue ? request.TimeZoneOffset.Value : 0),
                    Value = data.actual != null ? data.actual.ToString("N2") : null,
                    Target = data._target != null ? data._target.ToString("N2") : null,
                    Planned = data.planned != null ? data.planned.ToString("N2") : null,
                    Percentage = data.variance_percentage != null && data.variance_percentage != 0
                        ? data.variance_percentage.ToString("N2")
                        : null,
                    Variance = data.variance != null && data.variance != 0 ? data.variance.ToString("N2") : null,
                    Kbpi = data.kpbi != null
                        ? data.kpbi.ToString("N2")
                        : null,
                    Forecast = data.forecast != null ? data.forecast.ToString("N2") : null,
                });
            });
            return productLinkDetailDtos.OrderBy(c => c.DateData).Distinct().ToList();
        }

        public async Task<Response<ChartResponse>> GetChartData(ChartRequest request, CancellationToken token)
        {
            var chart = GetChart(request, token);
            var charDatas = new List<ChartData> { chart };
            return await Task.FromResult(Response.CreateResponse(new ChartResponse
            {
                ChartDatas = charDatas
            }));
        }
    }
}