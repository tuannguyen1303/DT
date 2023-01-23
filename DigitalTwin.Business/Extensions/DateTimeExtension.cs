using DigitalTwin.Common.Constants;

namespace DigitalTwin.Business.Extensions;

public static class DateTimeExtension
{
    public static string ToHyphenFormat(this DateTime dateTime) => dateTime.ToString(DateTimeFormat.HyphenYYYYMMDD);
    public static DateTime FirstDateOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);
    public static DateTime EndDateOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);

}