using System.Text.Json.Serialization;

namespace Modules.Users.Infrastructure.Dtos.Responses
{
    public class EmbedingResponseRepresentation
    {
        [JsonPropertyName("embedding")]
        public ICollection<float> Embedding { get; set; } = [];
    }
}