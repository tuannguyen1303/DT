using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigitalTwin.Common.JsonConverters
{
    /// <summary>
    /// Convert to UTC time if time kind == Unspecified <br/>
    /// Cover nulable DateTime case
    /// </summary>
    public class DateTimeNullableZeroTimeZone : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (!reader.TryGetDateTime(out DateTime timeParsed))
            {
                return null;
            }

            if (timeParsed.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(timeParsed, DateTimeKind.Utc);
            }

            return timeParsed.ToUniversalTime();
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? value,
            JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
            }
            if (value != null)
            {
                if (value.Value.Kind == DateTimeKind.Unspecified)
                {
                    value = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
                }
                writer.WriteStringValue(value.Value.ToUniversalTime());
            }
        }
    }
}