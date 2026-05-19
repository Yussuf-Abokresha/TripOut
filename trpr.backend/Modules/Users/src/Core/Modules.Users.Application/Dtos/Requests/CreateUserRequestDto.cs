using System.Text.Json.Serialization;
using Modules.Users.Application.Helpers;

namespace Modules.Users.Application.Dtos.Requests
{
    public class CreateUserRequestDto
    {
        public string Identifier { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [JsonConverter(typeof(MaskedStringConverter))]
        public string Password { get; set; } = string.Empty;
    }
}