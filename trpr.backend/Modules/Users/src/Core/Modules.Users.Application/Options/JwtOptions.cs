using System.ComponentModel.DataAnnotations;

namespace Modules.Users.Application.Options;

public class JwtOptions
{
    [Required]
    public string Secret { get; set; } = string.Empty;
}
