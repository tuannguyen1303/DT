namespace DigitalTwin.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Timezone convert
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="timeZoneOffset">In hours</param>
        /// <returns></returns>
        public static IDictionary<string, List<string>> updateDicValue12Months(this IDictionary<string, List<double>> currentDic,
            DateTime startedDate,
            int timeRangeIndex,
            int? timeZone)
        {
            startedDate = startedDate.AddHours(timeZone ?? 0);
            var firstMonth = 0;
            IDictionary<string, List<string>> resultDic = new Dictionary<string, List<string>>();

            foreach (var item in currentDic)
            {
                List<string> listValue12Months = new List<string>();
                for (int i = 0; i < 12; i++)
                {
                    listValue12Months.Add("N/A");
                }
                resultDic.Add(item.Key, new List<string>());
                var dicValue = currentDic[item.Key];
                for (int i = 0; i < timeRangeIndex; i++)
                {
                    if (startedDate.Month + i > 12) {
                        listValue12Months[firstMonth++] = dicValue[i].ToString();
                    }
                    else{
                        listValue12Months[startedDate.Month - 1 + i] = dicValue[i].ToString();
                    }
                }
                firstMonth = 0;
                resultDic[item.Key] = listValue12Months;
            }
            return resultDic;
        }
    }
}
