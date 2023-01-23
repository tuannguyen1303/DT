using System.Text;
using DigitalTwin.Common.Constants;

namespace DigitalTwin.Common.Utilities
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// if same year: <br/>
        /// Jan - Jun 2021 <br/>
        /// else: <br/>
        /// Jan 2021 - Jan 2022
        /// </summary>
        /// <param name="startedTime"></param>
        /// <param name="endedTime"></param>
        /// <param name="endedTimeFormat"></param>
        /// <param name="startedSameYearFormat"></param>
        /// <param name="startedDifferentYearFormat"></param>
        /// <returns></returns>
        public static string ConvertToTimeRangeString(DateTime startedTime, 
            DateTime endedTime, 
            string separator = "-",
            string endedTimeFormat = DateTimeFormat.MMMyyyy,
            string startedSameYearFormat = DateTimeFormat.MMM, 
            string startedDifferentYearFormat = DateTimeFormat.MMMyyyy)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(startedTime.Year == endedTime.Year ? 
                startedTime.ToString(startedSameYearFormat) : 
                startedTime.ToString(startedDifferentYearFormat));
            stringBuilder.Append(' ');
            stringBuilder.Append(separator);
            stringBuilder.Append(' ');
            stringBuilder.Append(endedTime.ToString(endedTimeFormat));
            return stringBuilder.ToString();
        }
    }
}
