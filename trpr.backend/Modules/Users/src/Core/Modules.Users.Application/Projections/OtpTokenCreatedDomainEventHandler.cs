using System.Net.Mail;
using Common.Application.DomainEvents;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Notifications.Contracts.Contracts;
using Modules.Users.Application.Abstractions;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Projections
{
    public class OtpTokenCreatedDomainEventHandler(IUsersDbContext usersDbContext, INotifiyContract notifiyContract) : IDomainEventHandler<TokenCreatedDomainEvent>
    {
        public async Task HandleAsync(TokenCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var token = await usersDbContext
                .Tokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(t => t.Id == domainEvent.TokenId, cancellationToken) ?? throw new NotFoundException("Token.NotFound", domainEvent.TokenId);
            if (token.Type == TokenType.Otp || token.Type == TokenType.ForgetPasswordOtp)
            {
                var emailAdress =
                    MailAddress.TryCreate(token.User.UserName, out var _) ?
                    token.User.UserName :
                    string.Format("{0}@trpr.com", token.User.UserName);
                await SendOtpMessage(emailAdress, token.Value, cancellationToken, token.Type);
            }
        }
        private async Task SendOtpMessage(string emailAdress, string otpValue, CancellationToken cancellationToken, TokenType tokenType)
        {
            var emailAdresses = emailAdress.EndsWith("@trpr.com") ? [] : new List<string> { emailAdress };
            var phoneNumbers = emailAdress.EndsWith("@trpr.com") ? new List<string> { emailAdress.Substring(0, emailAdress.Length - "@trpr.com".Length) } : [];
            await notifiyContract.NotifyAsync(new Notifications.Contracts.Dtos.SystemNotifyRequestDto(
                NotifyEmail: true,
                NotifyPhone: false,
                NotifySystem: false,
                TemplateType: TemplateTypeSwitch(tokenType),
                ToUserIds: [],
                ToEmails: emailAdresses,
                ToPhoneNumbers: phoneNumbers,
                KeyValuePairs: new Dictionary<string, string>
                {
                    { "code", otpValue }
                },
                LangCode: "en"
            ), cancellationToken);
        }

        public string TemplateTypeSwitch(TokenType tokenType) => tokenType switch
        {
            TokenType.Otp => "OtpMessage",
            TokenType.ForgetPasswordOtp => "ForgetPasswordMessage",
            _ => throw new NotSupportedException($"Token type {tokenType} is not supported for notification.")
        };
    }
}