using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SendPulseNetSDK.SendPulse.Helpers;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] formats = { "yyyy-MM-dd HH:mm:ss" };
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            DateTime.TryParseExact(reader.GetString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        throw new JsonException("Invalid date format.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}