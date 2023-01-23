using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigitalTwin.Common.JsonConverters
{
    /// <summary>
    /// Convert to UTC time if time kind == Unspecified
    /// </summary>
    public class DateTimeZeroTimeZone : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var stringReader = reader.GetString() ?? DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var timeParsed = DateTime.Parse(stringReader);
            if (timeParsed.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(timeParsed, DateTimeKind.Utc);
            }
            return timeParsed.ToUniversalTime();
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options)
        {
            if (value.Kind == DateTimeKind.Unspecified)
            {
                value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
            writer.WriteStringValue(value.ToUniversalTime());
        }
    }
}
