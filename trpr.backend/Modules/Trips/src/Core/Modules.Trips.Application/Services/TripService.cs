using Common.Application;
using Common.Application.Buckets;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Dtos;
using Modules.Trips.Application.Dtos.Requests;
using Modules.Trips.Application.Dtos.Responses;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public class TripService(IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory,
        PlaceService placeService,
        IMapper<Trip, TripResponseDto> tripMapper)
    {
        public async Task UpdateStatus(UpdateTripStatusRequestDto dto, CancellationToken cancellationToken)
        {
            var trip = await repositoryFactory.Repository<Trip>().GetFirstOrDefaultByFilter(t => t.Id == dto.Id)
                ?? throw new NotFoundException("Trip.NotFound", dto.Id);
            try
            {
                if (dto.IsApproved)
                    trip.Approve();
                else
                    trip.Reject(dto.RejectionReason ?? "No reason provided");
            }
            catch (InvalidOperationException ex)
            {
                throw new ConflictException(ex.Message);
            }
            repositoryFactory.Repository<Trip>().Update(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<TripResponseDto> CreateTrip(CreateTripRequestDto dto, Guid userId, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(User => User.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            var segments = await GetPlacesAsync(dto.Segments);
            var governorates = segments
                .SelectMany(x => x)
                .Select(x => x.Governorate)
                .DistinctBy(x => x.Id)
                .ToList();
            var trip = Trip.Create(
                        userId,
                        dto.ThemeId,
                        dto.Title,
                        dto.Description,
                        dto.Price,
                        dto.Images,
                        dto.TripVisibility,
                        dto.PublishMode,
                        segments,
                        dto.MaxParticipantsCount,
                        dto.GuideId,
                        dto.Segments.Select(s => s.Duration).ToList(),
                        user,
                        governorates);
            repositoryFactory.Repository<Trip>().Add(trip);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return tripMapper.Map(trip);
        }

        public async Task<PaginationDto<TripResponseDto>> GetTrips(SearchTripRequestDto request, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            // TODO FILTER BASED ON THE SEARCH REQUEST LATER
            var trips = repositoryFactory.Repository<Trip>().GetQueryable()
                .Include(x => x.Segments).ThenInclude(s => s.Places)
                .Include(x => x.Images)
                .Include(x => x.CreatedByUser)
                .AsQueryable();
            if (userId.HasValue)
                trips = trips.Where(x => x.UserId == userId.Value);
            if (request.PublishMode.HasValue)
                trips = trips.Where(x => x.PublishMode == request.PublishMode.Value);
            if (request.Status.HasValue)
                trips = trips.Where(x => x.Status == request.Status.Value);
            if (request.ThemeId.HasValue)
                trips = trips.Where(x => x.ThemeId == request.ThemeId.Value);
            if (request.GovernorateId.HasValue)
                trips = trips.Where(x => x.TripGovernorates.Any(g => g.GovernorateId == request.GovernorateId.Value));
            if (!string.IsNullOrEmpty(request.Title))
                trips = trips.Where(x => x.Title.Contains(request.Title));
            if (request.MinPrice.HasValue)
                trips = trips.Where(x => x.Price >= request.MinPrice.Value);
            if (request.MaxPrice.HasValue)
                trips = trips.Where(x => x.Price <= request.MaxPrice.Value);
            ICollection<Trip> pagedTrips = [];
            int count = 0;
            if (request.Page.HasValue && request.PageSize.HasValue)
            {
                pagedTrips = await trips
                    .Skip((request.Page.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .ToListAsync(cancellationToken);
                count = await trips.CountAsync(cancellationToken);
            }
            else
            {
                pagedTrips = await trips.ToListAsync(cancellationToken);
                count = pagedTrips.Count;
            }
            return new PaginationDto<TripResponseDto>
            {
                Items = pagedTrips.Select(tripMapper.Map).ToList(),
                Page = request.Page ?? 1,
                PageSize = request.PageSize ?? count,
                TotalItems = count
            };
        }
        #region Helpers
        private async Task<ICollection<ICollection<Place>>> GetPlacesAsync(ICollection<DayDto> source)
        {
            ICollection<ICollection<Place>> places = new List<ICollection<Place>>();
            foreach (var day in source)
            {
                ICollection<Place> dayPlaces = await placeService.GetPlacesAsync(day.PlacesIds);
                places.Add(dayPlaces);
            }
            return places;
        }
        #endregion
    }
}
