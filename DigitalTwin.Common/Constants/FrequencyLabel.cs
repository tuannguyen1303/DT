namespace DigitalTwin.Common.Constants
{
    public static class FrequencyLabel
    {
        public static List<Monthly> MonthlyLabels = new List<Monthly>()
        {
            new Monthly()
            {Month = "Jan", MonthNumb = 1 },
            new Monthly()
            {Month = "Feb", MonthNumb = 2 },
            new Monthly()
            {Month = "Mar", MonthNumb = 3 },
            new Monthly()
            {Month = "Apr", MonthNumb = 4 },
            new Monthly()
            {Month = "May", MonthNumb = 5 },
            new Monthly()
            {Month = "Jun", MonthNumb = 6 },
            new Monthly()
            {Month = "Jul", MonthNumb = 7 },
            new Monthly()
            {Month = "Aug", MonthNumb = 8 },
            new Monthly()
            {Month = "Sep", MonthNumb = 9 },
            new Monthly()
            {Month = "Oct", MonthNumb = 10 },
            new Monthly()
            {Month = "Nov", MonthNumb = 11 },
            new Monthly()
            {Month = "Dec", MonthNumb = 12 },
        };
        public static List<string> HourlyLabels = new List<string>()
        {
            "0:00",
            "1:00",
            "2:00",
            "3:00",
            "4:00",
            "5:00",
            "6:00",
            "7:00",
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00",
            "22:00",
            "23:00",
            "24:00",
        };
        public static List<string> QuarterlyLabels = new List<string>()
        {
            "Q1",
            "Q2",
            "Q3",
            "Q4"
        };
        public static List<string> RealTimeLabels = new List<string>()
        {
            "1",
            "2",
            "2",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13"
        };
        public static string Quarterly(int month)
        {
            switch (month)
            {
                default:
                    return "Q1";
                case 4 or 5 or 6:
                    return "Q2";
                case 7 or 8 or 9:
                    return "Q3";
                case 10 or 11 or 12:
                    return "Q4";
            }
        }
        public static string Hourly(int hour)
        {
            switch (hour)
            {
                default:
                    return "4:00";
                case int h when (h > 4 && h <= 8):
                    return "8:00";
                case int h when (h > 8 && h <= 12):
                    return "12:00";
                case int h when (h > 12 && h <= 16):
                    return "16:00";
                case int h when (h > 16 && h <= 20):
                    return "20:00";
                case int h when (h > 20 && h < 24):
                    return "24:00";
            }
        }
        public static int HourlyCurrentIndex(int hour)
        {
            switch (hour)
            {
                default:
                    return 1;
                case int h when (h > 4 && h <= 8):
                    return 2;
                case int h when (h > 8 && h <= 12):
                    return 3;
                case int h when (h > 12 && h <= 16):
                    return 4;
                case int h when (h > 16 && h <= 20):
                    return 5;
                case int h when (h > 20 && h < 24):
                    return 6;
            }
        }
        public static DateTime RoundUpDateTime(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        public static List<int> DailyLabels(int month, int year)
        {
            bool isLeapYear = false;
            if (year % 4 == 0)
            {
                if (year % 100 == 0)
                {
                    if (year % 400 == 0)
                        isLeapYear = true;
                    else
                        isLeapYear = false;
                }
                else
                    isLeapYear = true;
            }
            else
            {
                isLeapYear = false;
            }
            var defaultDays = new List<int>()
                    { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
            switch (month)
            {
                default:
                    return defaultDays;
                case 4 or 6 or 9 or 11:
                    {
                        var days = defaultDays;
                        days.Remove(31);
                        return days;
                    }
                case 2:
                    {
                        var days = defaultDays;
                        if (isLeapYear == false)
                        {
                            days.Remove(29);
                        }
                        days.Remove(30);
                        days.Remove(31);
                        return days;
                    }
            }
        }
        public static int QuarterlyIndex(int month)
        {
            switch (month)
            {
                default:
                    return 0;
                case 4 or 5 or 6:
                    return 1;
                case 7 or 8 or 9:
                    return 2;
                case 10 or 11 or 12:
                    return 3;
            }
        }
    }
    public class Monthly
    {
        public string? Month;
        public int? MonthNumb;
    }
    
}
