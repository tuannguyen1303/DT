namespace DigitalTwin.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// note: do not call this function in linq to sql query
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        public static string RemoveTransporterNameTail(this string name, string tail = "")
        {
            return name.Replace(tail, string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
        }

        public static bool In(this string name, List<string> listValues)
        {
            return listValues.Contains(name);
        }
    }
}
