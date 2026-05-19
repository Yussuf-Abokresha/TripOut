using Common.Application.Seeds;

namespace Api.Extensions;

public static class SeedingExtension
{
    public static async Task ExecuteSeedingAsync(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<ISeed>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}
