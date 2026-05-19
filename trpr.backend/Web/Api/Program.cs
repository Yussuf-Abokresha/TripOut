using Api.Extensions;
using Common.Application;
using Common.Infrastructure;
using Common.Presentation;
using Modules.Conversations.Infrastructure;
using Modules.Conversations.Presentation.Hubs;
using Modules.Notifications.Infrastructure;
using Modules.Trips.Infrastructure;
using Modules.Users.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureScalar();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddConfigurations(builder.Configuration,
    "Users",
    "Notifications",
    "Trips",
    "Conversations");
builder.Services.AddCommonApplication(
    Modules.Users.Application.AssemblyRefrence.Assembly,
    Modules.Notifications.Application.AssemblyRefrence.Assembly,
    Modules.Trips.Application.AssemblyRefrence.Assembly,
    Modules.Conversations.Application.AssemblyRefrence.Assembly);
builder.Services.AddCommonPresentation(
    Modules.Users.Presentation.AssemblyRefrence.Assembly,
    Modules.Notifications.Presentation.AssemblyRefrence.Assembly,
    Modules.Trips.Presentation.AssemblyRefrence.Assembly,
    Modules.Conversations.Presentation.AssemblyRefrence.Assembly);
builder.Services.AddCommonInfrastructure(builder.Configuration,
    Modules.Users.Infrastructure.AssemblyRefrence.Assembly,
    Modules.Notifications.Infrastructure.AssemblyRefrence.Assembly,
    Modules.Trips.Infrastructure.AssemblyRefrence.Assembly,
    Modules.Conversations.Infrastructure.AssemblyRefrence.Assembly);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddNotificationsModule(builder.Configuration);
builder.Services.AddTripsModule(builder.Configuration);
builder.Services.AddConversationsModule(builder.Configuration);
builder.Services.AddIntegrationEvents();
builder.Services.AddLocalizationServices();
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();
var app = builder.Build();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.AddMigrations();
    // await app.ExecuteSeedingAsync();
}
app.MapHub<ChatHub>("chat-hub");
app.UseLocalization();
app.AddMiddlewares();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
