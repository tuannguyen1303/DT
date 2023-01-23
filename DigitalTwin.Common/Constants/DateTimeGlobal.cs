namespace DigitalTwin.Common.Constants
{
    public static class DateTimeGlobal
    {
        public static DateTime SystemNow => DateTime.Now;

        public static DateTime InternationalNow => DateTime.UtcNow;

        public static readonly TimeSpan UTCOffset = TimeSpan.Zero;

        public static readonly int UTCHourOffset = 0;

        public static readonly int MonthsInYear = 12;

        public const string Jan = "JAN";

        public const string Feb = "FEB";

        public const string Mar = "MAR";

        public const string Apr = "APR";

        public const string May = "MAY";

        public const string Jun = "JUN";

        public const string Jul = "JUL";

        public const string Aug = "AUG";

        public const string Sep = "SEP";

        public const string Oct = "OCT";

        public const string Nov = "NOV";

        public const string Dec = "DEC";
    }
}
