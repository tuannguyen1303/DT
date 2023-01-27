using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Utilities;

namespace DigitalTwin.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// if same year: <br/>
        /// Jan - Jun 2021 <br/>
        /// else: <br/>
        /// Jan 2021 - Jan 2022
        /// </summary>
        /// <param name="startedTime"></param>
        /// <param name="endedTime"></param>
        /// <param name="separator"></param>
        /// <param name="endedTimeFormat"></param>
        /// <param name="startedSameYearFormat"></param>
        /// <param name="startedDifferentYearFormat"></param>
        /// <returns></returns>
        public static string ToTimeRangeString(this DateTime startedTime, DateTime endedTime,
            string separator = "-",
            string endedTimeFormat = DateTimeFormat.MMMyyyy,
            string startedSameYearFormat = DateTimeFormat.MMM,
            string startedDifferentYearFormat = DateTimeFormat.MMMyyyy)
        {
            return DateTimeUtils.ConvertToTimeRangeString(startedTime, endedTime,
                separator,
                endedTimeFormat,
                startedSameYearFormat,
                startedDifferentYearFormat);
        }

        /// <summary>
        /// To ISO 8601 string <br/>
        /// "2021-06-24T05:12:46.8778227Z"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToISOString(this DateTime dateTime)
        {
            return dateTime.ToString("o");
        }

        /// <summary>
        /// Timezone convert
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="timeZoneOffset">In hours</param>
        /// <returns></returns>
        public static DateTimeOffset ToTZ(this DateTime dateTime, int? timeZoneOffset)
        {
            var offset = TimeSpan.FromHours((timeZoneOffset ?? 0));
            if (dateTime.Kind != DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime
                    .ToUniversalTime()
                    .Add(offset), DateTimeKind.Unspecified);
            }
            var dateOffset = new DateTimeOffset(dateTime, offset);
            return dateOffset;
        }

        /// <summary>
        /// Timezone convert
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="timeZoneOffset"></param>
        /// <returns></returns>
        public static DateTimeOffset ToTZ(this DateTime dateTime, TimeSpan timeZoneOffset)
        {
            if (dateTime.Kind != DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime
                    .ToUniversalTime()
                    .Add(timeZoneOffset), 
                    DateTimeKind.Unspecified);
            }
            var dateOffset = new DateTimeOffset(dateTime, timeZoneOffset);
            return dateOffset;
        }

        /// <summary>
        /// ToUTC
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="fromTimeZoneOffset"></param>
        /// <returns></returns>
        public static DateTime ToUTC(this DateTime dateTime, TimeSpan fromTimeZoneOffset)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return dateTime.ToTZ(fromTimeZoneOffset).UtcDateTime;
            }
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// ToUTC
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="fromTimeZoneOffset"></param>
        /// <returns></returns>
        public static DateTime ToUTC(this DateTime dateTime, int? fromTimeZoneOffset)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return dateTime.ToTZ(fromTimeZoneOffset).UtcDateTime;
            }
            return dateTime.ToUniversalTime();
        }
        
        public static string ToHyphenFormat(this DateTime dateTime) => dateTime.ToString(DateTimeFormat.HyphenYYYYMMDD);
        
        public static DateTime FirstDateOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);
        
        public static DateTime EndDateOfMonth(this DateTime dateTime) =>
            new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
    }
}
