using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Modules.Users.Infrastructure.Dtos.Responses;

namespace Modules.Users.Infrastructure.Clients
{
    public class SemanticModelClient(HttpClient client)
    {
        private static readonly string endpoint = "embed";
        public async Task<EmbedingResponseRepresentation> GenerateEmbeding(string text)
        {
            string urlWithParams = QueryHelpers.AddQueryString(endpoint, new Dictionary<string, string?>()
            {
                {"text" , text}
            });
            var httpResponseMessage = await client.PostAsJsonAsync(urlWithParams, new { });
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadFromJsonAsync<EmbedingResponseRepresentation>() ?? throw new InvalidOperationException("Failed to read embeding list from response.");
        }
    }
}