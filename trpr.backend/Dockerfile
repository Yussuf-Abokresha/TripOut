FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files
COPY . .

# Restore dependencies
RUN dotnet restore "./Project.sln"

# Build and publish
RUN dotnet build "Web/Api/Api.csproj" -c Release --no-restore
RUN dotnet publish "Web/Api/Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Api.dll"] 