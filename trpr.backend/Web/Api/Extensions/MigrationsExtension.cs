using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using Modules.Conversations.Infrastructure.Data;
using Modules.Notifications.Infrastructure.Data;
using Modules.Trips.Infrastructure.Data;
using Modules.Users.Infrastructure.Data;

namespace Api.Extensions;

public static class MigrationsExtension
{
    //dotnet ef migrations add "IntialCreate" --project .\Modules\Conversations\src\External\Modules.Conversations.Infrastructure\ --startup-project .\Web\Api\ --context ConversationsDbContext

    //dotnet ef migrations add "IntialCreate" --project .\Modules\Notifications\src\External\Modules.Notifications.Infrastructure\ --startup-project .\Web\Api\ --context NotificationsDbContext

    //dotnet ef migrations add "InitialCreate" --project .\Modules\Trips\src\External\Modules.Trips.Infrastructure\ --startup-project .\Web\Api\ --context TripsDbContext

    //dotnet ef migrations add "IntialCreate" --project .\Modules\Users\src\External\Modules.Users.Infrastructure\ --startup-project .\Web\Api\ --context UsersDbContext
    public static async Task AddMigrations(this IApplicationBuilder application)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var usersDbContext = scope.ServiceProvider
            .GetRequiredService<UsersDbContext>();
        var notificationsDbContext = scope.ServiceProvider
            .GetRequiredService<NotificationsDbContext>();
        var tripsDbContext = scope.ServiceProvider
            .GetRequiredService<TripsDbContext>();
        var conversationsDbContext = scope.ServiceProvider
            .GetRequiredService<ConversationsDbContext>();
        usersDbContext.Database.Migrate();
        notificationsDbContext.Database.Migrate();
        tripsDbContext.Database.Migrate();
        conversationsDbContext.Database.Migrate();
        var minioClient = scope.ServiceProvider.GetRequiredService<IMinioClient>();
        await CreateBucketWithPoliciesAsync(minioClient, "uploads");
    }

    public static async Task CreateBucketWithPoliciesAsync(IMinioClient _minio, string bucketName)
    {
        bool exists = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName));

        if (!exists)
        {
            await _minio.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName));

            var policyJson = GeneratePolicy(bucketName);
            await _minio.SetPolicyAsync(
                new SetPolicyArgs()
                    .WithBucket(bucketName)
                    .WithPolicy(policyJson));
        }
    }

    private static string GeneratePolicy(string bucketName)
    {
        var policy = new
        {
            Version = "2012-10-17",
            Statement = new[]
            {
                new
                {
                    Effect = "Allow",
                    Principal = new { AWS = "*" },
                    Action = new[] { "s3:GetObject" },
                    Resource = new[]
                    {
                        $"arn:aws:s3:::{bucketName}/*"
                    }
                }
            }
        };

        return JsonSerializer.Serialize(policy);
    }
}
