# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-artistservice
WORKDIR /src

COPY ArtistService/ArtistService.csproj ArtistService/
RUN dotnet restore ArtistService/ArtistService.csproj

COPY Contracts/Contracts.csproj Contracts/

COPY . .
RUN dotnet publish ArtistService/ArtistService.csproj -c Release -o /app/out/ArtistService

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-artistservice
WORKDIR /app
COPY --from=build-artistservice /app/out/ArtistService ./

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "ArtistService.dll"]