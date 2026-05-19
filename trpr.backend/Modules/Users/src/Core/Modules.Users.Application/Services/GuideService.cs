using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Buckets;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Mappers;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.ValueObjects;
namespace Modules.Users.Application.Services
{
    public class GuideService(
        RepositoryFactory repositoryFactory,
        IUnitOfWork unitOfWork, 
        IMapper<ICollection<DocumentDto>,Dictionary<string, DocumentType>> mapper,
        IMapper<ICollection<Document>, ICollection<DocumentDto>> documentMapper)
    {
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpgradeToGuide(Guid userId, GuideUpgradeRequestDto request, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            var documents = mapper.Map(request.Documents);
            GuideUpgradeRequest upgradeRequest = GuideUpgradeRequest.Create(userId, documents);
            repositoryFactory.Repository<GuideUpgradeRequest>().Add(upgradeRequest);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return upgradeRequest.ToResponseDto(documentMapper.Map(upgradeRequest.Documents));
        }
        public async Task<PaginationDto<UpgradePaginationResponseDto>> AllUpgradeRequests(UpgradePaginationRequestDto dto, CancellationToken cancellationToken)
        {
            var query = repositoryFactory.Repository<GuideUpgradeRequest>().GetQueryable();
            query = query.Where(x => x.Status == dto.status);
            if (dto.sortByUpdatedate)
                query = query.OrderByDescending(x => x.UpdatedAtUTC);
            else
                query = query.OrderByDescending(x => x.CreatedAtUTC);
            int TotalItems = await query.CountAsync(cancellationToken);

            List<UpgradePaginationResponseDto> items = await query.Include(x => x.user)
                .Select(x => x.ToResponseDto())
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync(cancellationToken);
            return PaginationDto<UpgradePaginationResponseDto>.Create(dto.Page, dto.PageSize, TotalItems, items);
        }

        public async Task<GuideUpgradeResponseDto> ChangeGuideStatus(Guid AdminId, UpdateGuideStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var upgradeRequest = await repositoryFactory.Repository<GuideUpgradeRequest>()
                .GetFirstOrDefaultByFilter(u => u.userId == dto.UserId && u.Id == dto.UpgradeRequestId)
                ?? throw new NotFoundException("UpgradeRequest.NotFound", dto.UpgradeRequestId);
            if(upgradeRequest.Status == ApproveStatus.Pending && dto.Status != ApproveStatus.Pending)
            {
                upgradeRequest.UpdateStatus(AdminId, upgradeRequest.userId, dto.Status, dto.RejectionReason);
            }
            repositoryFactory.Repository<GuideUpgradeRequest>().Update(upgradeRequest);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return upgradeRequest.ToResponseDto(documentMapper.Map(upgradeRequest.Documents));
        }

        public async Task<List<GuideUpgradeResponseDto>> UserUpgradeRequests(Guid upgradeRequestId, CancellationToken cancellationToken)
        {
            var request = await repositoryFactory.Repository<GuideUpgradeRequest>()
                .GetFirstOrDefaultByFilter(u => u.Id == upgradeRequestId)
                ?? throw new NotFoundException("UpgradeRequest.NotFound", upgradeRequestId);
            var query = await repositoryFactory.Repository<GuideUpgradeRequest>().GetQueryable()
                    .Where(u => u.userId == request.userId)
                    .OrderBy(u => u.Status)
                    .Include(u => u.Documents)
                    .Include(u => u.user)
                    .ToListAsync(cancellationToken);  
            var results = query
                .Select(ur => ur.ToResponseDto(
                    documentMapper.Map(ur.Documents)
                ))
                .ToList();
            return results;
        }
    }
}
