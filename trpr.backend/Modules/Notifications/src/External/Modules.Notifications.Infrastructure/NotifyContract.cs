using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Contracts.Contracts;
using Modules.Notifications.Contracts.Dtos;
using Modules.Notifications.Domain.Entities;
using Modules.Notifications.Domain.ValueObjects;

namespace Modules.Notifications.Infrastructure;

public class NotifyContract(
    ILogger<NotifyContract> logger,
    RepositoryFactory repositoryFactory,
    IUnitOfWork unitOfWork) : INotifiyContract
{
    public async Task NotifyAsync(SystemNotifyRequestDto request, CancellationToken cancellationToken)
    {
        var templateType = Enum.TryParse(typeof(TemplateType), request.TemplateType, out var result) ? (TemplateType)result : throw new ArgumentException("Invalid template type");
        var template = await repositoryFactory.Repository<Template>().GetQueryable()
            .Include(x => x.TemplateLangs)
            .Where(x => x.TemplateType == templateType && x.Active == true)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new NotFoundException("Template.NotFound", templateType.ToString());
        var templateLange = template
            .TemplateLangs
            .FirstOrDefault(x => x.LangCode == request.LangCode) ??
            template
            .TemplateLangs
            .FirstOrDefault(x => x.LangCode == "en") ??
            throw new NotFoundException("Template.Lang.NotFound", templateType, request.LangCode);
        ICollection<User> users =
            repositoryFactory.Repository<User>().GetQueryable()
            .Where(x => request.ToUserIds.Contains(x.Id))
            .ToList();
        await NotifyUsers(request, users, template.ContentType, templateLange.Content, cancellationToken);
        NotifyEmails(request, templateLange.Content, cancellationToken);
        NotifyPhones(request, templateLange.Content, cancellationToken);
    }
    private void NotifyPhones(SystemNotifyRequestDto request, string content, CancellationToken cancellationToken)
    {
        var paredTemplate = Scriban.Template.Parse(content);
        var renderedContent = paredTemplate.Render(request.KeyValuePairs);

        foreach (var phone in request.ToPhoneNumbers)
        {
            logger.LogInformation("Sending SMS to {PhoneNumber} with content: {Content}", phone, renderedContent);
        }
    }

    private void NotifyEmails(SystemNotifyRequestDto request, string content, CancellationToken cancellationToken)
    {
        var paredTemplate = Scriban.Template.Parse(content);
        var renderedContent = paredTemplate.Render(request.KeyValuePairs);

        foreach (var email in request.ToEmails)
        {
            logger.LogInformation("Sending Email to {Email} with content: {Content}", email, renderedContent);
        }
    }

    private async Task NotifyUsers(SystemNotifyRequestDto request, ICollection<User> users, ContentType contentType, string content, CancellationToken cancellationToken)
    {
        foreach (var user in users)
        {
            var paredTemplate = Scriban.Template.Parse(content);
            IEnumerable<KeyValuePair<string, string>> newKeyValuePairs = new Dictionary<string, string>(request.KeyValuePairs)
            {
                ["FirstName"] = user.FirstName,
                ["LastName"] = user.LastName,
                ["UserName"] = user.UserName,
                ["Email"] = user.Email ?? string.Empty,
                ["PhoneNumber"] = user.PhoneNumber ?? string.Empty
            };
            newKeyValuePairs = newKeyValuePairs.Union(request.KeyValuePairs);
            var renderedContent = paredTemplate.Render(newKeyValuePairs);

            var notification = Notification.Create(
                renderedContent,
                contentType,
                request.NotifyEmail,
                request.NotifyPhone,
                request.NotifySystem,
                user.Id
            );
            repositoryFactory.Repository<Notification>().Add(notification);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

    }

    public async Task UpdateNotificationSettingsAsync(Guid userId, UpdateNotificationSettingsRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId) ?? throw new NotFoundException("User.NotFound", userId);
        user.UpdateNotificationSettings(
            request.TripUpdates,
            request.Messages,
            request.Promotions
        );
        repositoryFactory.Repository<User>().Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationSettingsResponseDto> GetNotificationSettingsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(x => x.Id == userId) ?? throw new NotFoundException("User.NotFound", userId);
        return new NotificationSettingsResponseDto
        {
            TripUpdates = user.TripUpdates,
            Messages = user.Messages,
            Promotions = user.Promotions
        };
    }
}
