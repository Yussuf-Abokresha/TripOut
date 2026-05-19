using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Mappers;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services;

public class PlaceService(IUnitOfWork unitOfWork, RepositoryFactory repositoryFactory)
{
    public async Task<PlaceFormDataDto> GetPlaceFormDataAsync(CancellationToken cancellationToken = default)
    {
        var categories = await repositoryFactory.Repository<Category>().GetQueryable()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync(cancellationToken);

        var governorates = await repositoryFactory.Repository<Governorate>().GetQueryable()
            .Select(g => new GovernorateDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync(cancellationToken);

        var tags = await repositoryFactory.Repository<Tag>().GetQueryable()
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync(cancellationToken);

        return new PlaceFormDataDto
        {
            Categories = categories,
            Governorates = governorates,
            Tags = tags
        };
    }
    public async Task<PlaceDto> UpdatePlaceAsync(int id, UpdatePlaceRequestDto dto, CancellationToken cancellationToken = default)
    {
        var place = await repositoryFactory.Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == id,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound", id);

        if (dto.CategoryId != null)
        {
            var category = await repositoryFactory
                .Repository<Category>()
                .GetFirstOrDefaultByFilter(c => c.Id == dto.CategoryId)
                ?? throw new NotFoundException("Category.NotFound", dto.CategoryId);
            place.CategoryId = dto.CategoryId.Value;
        }

        if (dto.GovernorateId != null)
        {
            var governorate = await repositoryFactory
                .Repository<Governorate>()
                .GetFirstOrDefaultByFilter(g => g.Id == dto.GovernorateId)
                ?? throw new NotFoundException("Governorate.NotFound", dto.GovernorateId);
            place.GovernorateId = dto.GovernorateId.Value;
        }

        if (dto.TagIds != null)
        {
            foreach (var tagId in dto.TagIds)
                _ = await repositoryFactory.Repository<Tag>().GetFirstOrDefaultByFilter(t => t.Id == tagId)
                        ?? throw new NotFoundException("Tag.NotFound", tagId);

            place.PlaceTags.Clear();
            foreach (var tagId in dto.TagIds)
            {
                var placeTag = new PlaceTag { PlaceId = place.Id, TagId = tagId };
                place.PlaceTags.Add(placeTag);
            }
        }

        if (dto.Title != null)
            place.Title = dto.Title;
        if (dto.Description != null)
            place.Description = dto.Description;
        if (dto.Longitude != null && dto.Latitude != null)
            place.Location = Place.Create(
                "dummy",
                "dummy",
                1,
                1,
                dto.Longitude.Value,
                dto.Latitude.Value).Location;

        repositoryFactory.Repository<Place>().Update(place);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        place = await repositoryFactory
            .Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == place.Id,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound", place.Id);

        return place.ToPlaceDto();
    }
    public async Task<PlaceDto> CreatePlaceAsync(CreatePlaceRequestDto dto, CancellationToken cancellationToken = default)
    {
        var category = await repositoryFactory
            .Repository<Category>()
            .GetFirstOrDefaultByFilter(c => c.Id == dto.CategoryId)
            ?? throw new NotFoundException("Category.NotFound", dto.CategoryId);

        var governorate = await repositoryFactory
            .Repository<Governorate>()
            .GetFirstOrDefaultByFilter(g => g.Id == dto.GovernorateId)
            ?? throw new NotFoundException("Governorate.NotFound", dto.GovernorateId);

        foreach (var tagId in dto.TagIds)
            _ = await repositoryFactory.Repository<Tag>().GetFirstOrDefaultByFilter(t => t.Id == tagId)
                    ?? throw new NotFoundException("Tag.NotFound", tagId);

        var place = Place.Create(
            dto.Title,
            dto.Description,
            dto.CategoryId,
            dto.GovernorateId,
            dto.Longitude,
            dto.Latitude);

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            repositoryFactory.Repository<Place>().Add(place);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            foreach (var tagId in dto.TagIds)
            {
                var placeTag = new PlaceTag { PlaceId = place.Id, TagId = tagId };
                place.PlaceTags.Add(placeTag);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        place = await repositoryFactory
            .Repository<Place>()
            .GetFirstOrDefaultByFilter(p => p.Id == place.Id,
                x => x
                    .Include(x => x.Governorate)
                    .Include(x => x.Category)
                    .Include(x => x.PlaceTags).ThenInclude(x => x.Tag))
            ?? throw new NotFoundException("Place.NotFound", place.Id);

        return place.ToPlaceDto();
    }
    public async Task<ICollection<PlaceDto>> GetPlacesAsync(GetPlacesQueryDto query, CancellationToken cancellationToken = default)
    {
        var queryable = repositoryFactory
            .Repository<Place>()
            .GetQueryable();
        if (query.GovernorateId != null)
            queryable = queryable.Where(x => x.GovernorateId == query.GovernorateId);
        if (query.Longitude != null && query.Latitude != null && query.RadiusInMeters != null)
        {
            var place = Place.Create(
                "dummy",
                "dummy",
                1,
                1,
                query.Longitude.Value,
                query.Latitude.Value);
            var point = place.Location;
            queryable = queryable.Where(x => x.Location.Distance(point) <= query.RadiusInMeters.Value);
        }
        queryable = queryable
            .Include(x => x.Category)
            .Include(x => x.Governorate)
            .Include(x => x.PlaceTags).ThenInclude(x => x.Tag);
        return await queryable
            .Select(x => x.ToPlaceDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<Place>> GetPlacesAsync(ICollection<int> placeIds)
    {
        ICollection<Place> places = [];
        foreach (var placeId in placeIds)
        {
            var place = await
                repositoryFactory
                .Repository<Place>()
                .GetFirstOrDefaultByFilter(
                    p => p.Id == placeId,
                    includes: x => x.Include(p => p.Governorate)
                        .Include(p => p.Category)
                        .Include(p => p.PlaceTags).ThenInclude(p => p.Tag))
                ?? throw new NotFoundException("Place.NotFound", placeId);
            places.Add(place);
        }
        return places;
    }
}
