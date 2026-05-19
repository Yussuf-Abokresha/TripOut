using System.Linq;
using System.Threading;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Notifications.Application.Abstractions;
using Modules.Notifications.Application.Dtos;
using Modules.Notifications.Application.Dtos.Requests;
using Modules.Notifications.Application.Dtos.Responses;
using Modules.Notifications.Application.Mappers;
using Modules.Notifications.Domain.Entities;


namespace Modules.Notifications.Application.Services
{
    public class TemplateService(IUnitOfWork unitOfWork, RepositoryFactory repositoryFactory)
    {
        public async Task<TemplateResponseDto> CreateTemplate(Guid? userId, CreateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {

            var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
            var titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
            var contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);

            Template template = Template.Create(templateDto.ContentType, templateDto.TemplateType, userId, contentLanguageDict, titleLanguageDict);
            repositoryFactory.Repository<Template>().Add(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return template.ToResponseDto();
        }
        public async Task<TemplateResponseDto> UpdateTemplate(Guid templateId, Guid? userId, UpdateTemplateDto templateDto, CancellationToken cancellationToken = default)
        {
            Template template = await repositoryFactory.Repository<Template>()
                .GetFirstOrDefaultByFilter(t => t.Id == templateId && t.UserId == userId)
                ?? throw new NotFoundException("Template.NotFound", templateId);
            if (templateDto.Active == true && !template.Active)
            {
                var activeTemplate = await repositoryFactory.Repository<Template>()
                    .GetFirstOrDefaultByFilter(x =>
                        x.UserId == template.UserId &&
                        x.Active &&
                        x.Id != templateId);
                if (activeTemplate != null)
                {
                    activeTemplate.Active = false;
                }
            }

            Dictionary<string, string>? titleLanguageDict = null;
            Dictionary<string, string>? contentLanguageDict = null;

            if (templateDto.Translations != null)
            {
                var templateLanguageDict = templateDto.Translations.ToDictionary(m => m.LangCode, x => x);
                titleLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Title);
                contentLanguageDict = templateLanguageDict.ToDictionary(m => m.Key, x => x.Value.Content);
            }

            template.Update(templateDto.ContentType, templateDto.TemplateType, contentLanguageDict, titleLanguageDict, templateDto.Active);
            repositoryFactory.Repository<Template>().Update(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return template.ToResponseDto();

        }

        public async Task<PaginationDto<TemplatePaginationResponseDto>> TemplatesPagination(PaginateRequestDto dto, Guid? userId, string LangCode, CancellationToken cancellationToken = default)
        {
            var query = repositoryFactory.Repository<Template>().GetQueryable();

            if (userId.HasValue)
                query = query.Where(t => t.UserId == userId);

            query = query.Where(t => t.TemplateLangs.Any(tl => tl.LangCode.Equals(LangCode)));
            if (dto.IsActive != null)
            {
                query = query.Where(t => t.Active == dto.IsActive);
            }
            if (dto.TemplateType.HasValue)
            {
                query = query.Where(t => t.TemplateType == dto.TemplateType);
            }
            if (!string.IsNullOrEmpty(dto.Search))
            {
                query = query.Where(t => t.TemplateLangs.Any(tl => tl.Title.StartsWith(dto.Search)));
            }
            if (dto.sortBy.HasValue)
            {
                query = dto.sortBy.Value switch
                {
                    SortBy.CreatedAt => query.OrderByDescending(t => t.CreatedAtUTC),
                    SortBy.UpdatedAt => query.OrderByDescending(t => t.UpdatedAtUTC),
                    _ => query.OrderByDescending(t => t.UpdatedAtUTC)
                };
            }
            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(x => x.User)
                .Include(t => t.TemplateLangs.Where(tl => tl.LangCode == LangCode))
                .Select(x => x.ToPaginationResponseDto())
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(cancellationToken);
            return PaginationDto<TemplatePaginationResponseDto>.Create(dto.Page, dto.PageSize, totalItems, items);
        }
        public async Task<TemplateResponseDto> TemplateDetails(Guid templateId, Guid? userId, CancellationToken cancellationToken)
        {
            Template template = await repositoryFactory.Repository<Template>()
                    .GetFirstOrDefaultByFilter(
                        t => t.Id == templateId && (userId == null || t.UserId == userId),
                        q => q.Include(t => t.TemplateLangs),
                        q => q.Include(t => t.User)
                    ) ?? throw new NotFoundException("Template.NotFound");
            return template.ToResponseDto();
        }
    }
}