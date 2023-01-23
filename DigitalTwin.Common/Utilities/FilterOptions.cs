namespace DigitalTwin.Common.Utilities
{
    public class FilterOptions
    {
        public string PropertyName { get; set; }

        public IEnumerable<string> FilterValues { get; set; }

        public FilterOptions(string propertyName, IEnumerable<string> filterValues)
        {
            PropertyName = propertyName;
            FilterValues = filterValues;
        }
    }
}
