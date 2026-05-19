using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Common.Application.Seeds;
using Microsoft.Extensions.Logging;
using Modules.Trips.Application.Abstractions;
using Modules.Trips.Application.Repositories;
using Modules.Trips.Domain.Entities;
using Modules.Trips.Infrastructure.Seeds.Dto;
namespace Modules.Trips.Infrastructure.Seeds;

public class PlacesCsvSeeder(ILogger<PlacesCsvSeeder> logger, RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : ISeed
{
    public async Task SeedAsync()
    {
        var seedsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Seeds");


        var placeFiles = Directory.GetFiles(seedsDirectory, "places_*.csv");
        // Optional: sort them if order matters (e.g. version order)
        var orderedFiles = placeFiles
            .OrderBy(f =>
            {
                var fileName = Path.GetFileNameWithoutExtension(f); // places_1.0.1
                var versionPart = fileName.Replace("places_", "");
                return Version.Parse(versionPart);
            })
            .ToList();

        foreach (var file in orderedFiles)
        {
            var seedHistory = await repositoryFactory.Repository<CsvSeedHistory>()
                .GetFirstOrDefaultByFilter(s => s.FileName == Path.GetFileName(file));

            if (seedHistory != null)
            {
                logger.LogInformation("Seed file already processed, skipping: {FileName}", file);
                continue;
            }

            logger.LogInformation("Processing seed file: {FileName}", file);
            await ProcessCsvFile(file);

            repositoryFactory
                .Repository<CsvSeedHistory>()
                .Add(CsvSeedHistory.Create(Path.GetFileName(file)));
            await unitOfWork.SaveChangesAsync();
        }
    }

    private async Task ProcessCsvFile(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<PlaceMap>();

        var records = csv.GetRecords<PlaceCsv>().ToList();

        var tagNames = records
            .Select(x => x.Tags)
            .SelectMany(x => x.Split(','))
            .Select(t => t.Trim().ToLower())
            .Distinct()
            .ToList();

        var governorateNames = records
            .Select(x => x.Governorate)
            .Select(g => g.Trim().ToLower())
            .Distinct()
            .ToList();

        var categoryNames = records
            .Select(x => x.CategoryClean)
            .Select(c => c.Trim().ToLower())
            .Distinct()
            .ToList();
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var tags = await SeedTags(tagNames);
            var NameToTagMapper = tags.ToDictionary(t => t.Name, t => t);
            var governorates = await SeedGovernorates(governorateNames);
            var NameToGovernorateMapper = governorates.ToDictionary(g => g.Name, g => g);
            var categories = await SeedCategories(categoryNames);
            var NameToCategoryMapper = categories.ToDictionary(c => c.Name, c => c);
            foreach (var record in records)
            {
                var place = Place.Create(
                    record.PlaceName,
                    record.Description,
                    NameToCategoryMapper[record.CategoryClean.Trim().ToLower()].Id,
                    NameToGovernorateMapper[record.Governorate.Trim().ToLower()].Id,
                    record.Lon,
                    record.Lat);

                var placeTags = record.Tags
                    .Split(',')
                    .Select(t => t.Trim().ToLower())
                    .Distinct()
                    .Select(t => new PlaceTag
                    {
                        Place = place,
                        Tag = NameToTagMapper[t]
                    })
                    .ToList();

                place.PlaceTags = placeTags;

                repositoryFactory.Repository<Place>().Add(place);

                await unitOfWork.SaveChangesAsync();

                logger.LogInformation("Read place: {PlaceName} at ({Lat}, {Lon}) with tags: {Tags}",
                    record.PlaceName, record.Lat, record.Lon, record.Tags);
            }
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding places from file: {FileName}", path);
            await unitOfWork.RollbackTransactionAsync();
        }
    }
    private async Task<ICollection<Governorate>> SeedGovernorates(ICollection<string> governorateNames, CancellationToken cancellationToken = default)
    {
        var governorateRepository = repositoryFactory.Repository<Governorate>();

        var existingGovernorates = await governorateRepository
            .GetByExpWhereAsync(x => governorateNames.Contains(x.Name));

        var existingGovernorateNames = existingGovernorates
            .Select(x => x.Name)
            .ToHashSet();

        var newGovernorates = governorateNames
            .Where(x => !existingGovernorateNames.Contains(x))
            .Select(x => new Governorate
            {
                Id = 0,
                Name = x
            })
            .ToList();

        if (newGovernorates.Count > 0)
        {
            var invalidGovernorateNames = string.Join(", ", newGovernorates.Select(g => g.Name));
            throw new Exception($"Governorates Names are invalid or not exist in the database, please add them first before seeding places: ({invalidGovernorateNames})");
        }
        return existingGovernorates;
    }

    private async Task<ICollection<Tag>> SeedTags(
    ICollection<string> tagNames,
    CancellationToken cancellationToken = default)
    {
        var tagRepository = repositoryFactory.Repository<Tag>();

        var existingTags = await tagRepository
            .GetByExpWhereAsync(x => tagNames.Contains(x.Name));

        var existingTagNames = existingTags
            .Select(x => x.Name)
            .ToHashSet();

        var newTags = tagNames
            .Where(x => !existingTagNames.Contains(x))
            .Select(x => new Tag
            {
                Id = 0,
                Name = x
            })
            .ToList();

        if (newTags.Count > 0)
        {
            tagRepository.AddRange(newTags);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // IMPORTANT:
        // Reload from database so EF tracks only one instance
        var allTags = await tagRepository
            .GetByExpWhereAsync(x => x.Name != null);

        return allTags.ToList();
    }

    private async Task<ICollection<Category>> SeedCategories(ICollection<string> categoryNames, CancellationToken cancellationToken = default)
    {
        var categoryRepository = repositoryFactory.Repository<Category>();

        var existingCategories = await categoryRepository
            .GetByExpWhereAsync(x => categoryNames.Contains(x.Name));

        var existingCategoryNames = existingCategories
            .Select(x => x.Name)
            .ToHashSet();

        var newCategories = categoryNames
            .Where(x => !existingCategoryNames.Contains(x))
            .Select(x => new Category
            {
                Name = x
            })
            .ToList();

        if (newCategories.Count > 0)
        {
            var invalidCategoryNames = string.Join(", ", newCategories.Select(c => c.Name));
            throw new Exception($"Category Names are invalid or not exist in the database, please add them first before seeding places: ({invalidCategoryNames})");
        }
        return existingCategories;
    }
}
