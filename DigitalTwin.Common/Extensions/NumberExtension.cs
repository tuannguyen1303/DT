namespace DigitalTwin.Common.Extensions
{
    public static class NumberExtension
    {
        public static double Round(this double floatPointNumber,
            int digits,
            MidpointRounding midpointRounding = MidpointRounding.ToEven)
        {
            return Math.Round(floatPointNumber, digits, midpointRounding);
        }
        
        public static double Round(this float floatPointNumber,
            int digits,
            MidpointRounding midpointRounding = MidpointRounding.ToEven)
        {
            return Math.Round(floatPointNumber, digits, midpointRounding);
        }

        /// <summary>
        /// Compare percent of compareNum in compareTotal
        /// </summary>
        /// <returns></returns>
        public static double? ComparePercent(this int compareNum1, int compareNum2)
        {
            double? nullNumber = null;
            if (compareNum1 == 0 && compareNum2 == 0) {
                return 0;
            }
            return compareNum2 == 0 ? nullNumber : (compareNum1 * 100 / compareNum2);
        }
    }
}
