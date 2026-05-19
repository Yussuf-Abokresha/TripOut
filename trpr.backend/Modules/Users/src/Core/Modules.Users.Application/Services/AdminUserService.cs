using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Enums;

namespace Modules.Users.Application.Services;

public class AdminUserService(
    RepositoryFactory repositoryFactory,
    IUnitOfWork unitOfWork)
{
    public async Task<PaginationDto<UserResponseDto>> GetUsersAsync(GetUsersRequestDto request, CancellationToken cancellationToken = default)
    {
        var query = repositoryFactory.Repository<User>().GetQueryable()
            .Include(u => u.UserRoles)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Languages).ThenInclude(pl => pl.Language)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Interests).ThenInclude(pi => pi.Interest)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Vibes).ThenInclude(pv => pv.Vibe)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(u => u.UserName.Contains(request.Search));

        int totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(u => u.UserName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => MapToDto(u))
            .ToListAsync(cancellationToken);

        return PaginationDto<UserResponseDto>.Create(request.Page, request.PageSize, totalItems, items);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetQueryable()
            .Include(u => u.UserRoles)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Languages).ThenInclude(pl => pl.Language)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Interests).ThenInclude(pi => pi.Interest)
            .Include(u => u.Profile)
                .ThenInclude(p => p.Vibes).ThenInclude(pv => pv.Vibe)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new NotFoundException("User.NotFound", userId);

        return MapToDto(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
            ?? throw new NotFoundException("User.NotFound", userId);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        repositoryFactory.Repository<User>().Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetUserByIdAsync(userId, cancellationToken);
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
            ?? throw new NotFoundException("User.NotFound", userId);

        repositoryFactory.Repository<User>().Delete(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserResponseDto> AssignRolesAsync(Guid userId, AssignRolesRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await repositoryFactory.Repository<User>().GetQueryable()
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new NotFoundException("User.NotFound", userId);

        var existingRoles = user.UserRoles.Select(ur => ur.Role).ToHashSet();
        var requestedRoles = request.Roles.ToHashSet();

        // Remove roles that are no longer in the request
        var rolesToRemove = user.UserRoles.Where(ur => !requestedRoles.Contains(ur.Role)).ToList();
        foreach (var roleToRemove in rolesToRemove)
            repositoryFactory.Repository<UserRole>().Delete(roleToRemove);

        // Add new roles
        foreach (var role in requestedRoles.Where(r => !existingRoles.Contains(r)))
            repositoryFactory.Repository<UserRole>().Add(new UserRole { UserId = userId, Role = role });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetUserByIdAsync(userId, cancellationToken);
    }

    private static UserResponseDto MapToDto(User user)
    {
        ProfileResponseDto? profileDto = null;

        if (user.Profile != null)
        {
            profileDto = new ProfileResponseDto
            {
                Id = user.Profile.Id,
                Languages = user.Profile.Languages.Select(pl => new LanguageResponseDto
                {
                    Id = pl.Language.Id,
                    Name = pl.Language.Name,
                    Code = pl.Language.Code,
                    NativeName = pl.Language.NativeName,
                    Icon = pl.Language.Icon
                }).ToList(),
                Interests = user.Profile.Interests.Select(pi => new InterestResponseDto
                {
                    Id = pi.Interest.Id,
                    Icon = pi.Interest.Icon,
                    Name = pi.Interest.Name
                }).ToList(),
                Vibes = user.Profile.Vibes.Select(pv => new VibeResponseDto
                {
                    Id = pv.Vibe.Id,
                    Thumbnail = pv.Vibe.Thumbnail,
                    Description = pv.Vibe.Description,
                    Name = pv.Vibe.Name
                }).ToList()
            };
        }

        return new UserResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsVerified = user.IsVerified,
            TwoFactorEnabled = user.TwoFactorEnabled,
            Roles = user.UserRoles.Select(ur => ur.Role.ToString()).ToList(),
            Profile = profileDto
        };
    }
}
