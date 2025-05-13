# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-artistservice
WORKDIR /app

COPY ArtistService/ArtistService.csproj ArtistService/
COPY Contracts/Contracts.csproj Contracts/

RUN dotnet restore ArtistService/ArtistService.csproj
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-artistservice
WORKDIR /app
COPY --from=build-artistservice /app/out ./

# Expose the port and define the entry point
EXPOSE 80 5672
ENTRYPOINT ["dotnet", "ArtistService.dll"]