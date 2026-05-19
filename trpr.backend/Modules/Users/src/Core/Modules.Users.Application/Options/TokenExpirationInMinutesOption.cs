namespace Modules.Users.Application.Options;

public class TokenExpirationInMinutesOption
{
    public int Otp { get; set; } = 3;
    public int ForgetPasswordOtp { get; set; } = 3;
}
