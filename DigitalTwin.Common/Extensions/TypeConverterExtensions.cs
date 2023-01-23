using System.Text.Json;

namespace DigitalTwin.Common.Extensions
{
    public static class TypeConverterExtensions
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T? Convert<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }
    }
}
