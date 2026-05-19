namespace Modules.Users.Application.Dtos.Requests
{
    public class VerifyOtpRequestDto
    {
        public Guid Identifier { get; set; }
        public string Value { get; set; } = string.Empty;
    }

}