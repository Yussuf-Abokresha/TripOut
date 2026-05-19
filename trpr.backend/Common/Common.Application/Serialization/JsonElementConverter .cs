using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Common.Application.Serialization;

public class JsonElementConverter : JsonConverter<JsonElement>
{
    public override JsonElement ReadJson(
        JsonReader reader,
        Type objectType,
        JsonElement existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return default;
        }

        var token = JToken.Load(reader);
        using var doc = JsonDocument.Parse(token.ToString());
        return doc.RootElement.Clone();
    }

    public override void WriteJson(JsonWriter writer, JsonElement value, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (value.ValueKind == JsonValueKind.Undefined)
        {
            writer.WriteNull();
            return;
        }

        var token = JToken.Parse(value.GetRawText());
        token.WriteTo(writer);
    }

}