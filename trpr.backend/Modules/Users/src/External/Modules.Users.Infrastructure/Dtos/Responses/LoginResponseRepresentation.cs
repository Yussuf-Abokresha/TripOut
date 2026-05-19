using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Modules.Users.Infrastructure.Dtos.Responses
{
    public class LoginResponseRepresentation
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}