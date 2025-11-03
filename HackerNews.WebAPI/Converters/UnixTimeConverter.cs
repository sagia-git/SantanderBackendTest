using Newtonsoft.Json;

namespace HackerNews.WebAPI.Converters;

public class UnixTimeConverter : JsonConverter<DateTimeOffset>
{
    public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToUnixTimeSeconds());
    }

    public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var unixTime = Convert.ToInt64(reader.Value);
        return DateTimeOffset.FromUnixTimeSeconds(unixTime);
    }
}
