using DigitalTwin.Common.Enums;

namespace DigitalTwin.Common.Utilities
{
    public class SortingOptions
    {
        public string Column { get; set; }

        public SortingOptions(string column)
        {
            Column = column;
        }

        public ESortingTypes SortType { get; set; }
    }
}