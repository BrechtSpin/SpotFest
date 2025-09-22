# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-happeningservice
WORKDIR /src

COPY HappeningService/HappeningService.csproj HappeningService/
RUN dotnet restore HappeningService/HappeningService.csproj

COPY Contracts/Contracts.csproj Contracts/

COPY . .
RUN dotnet publish HappeningService/HappeningService.csproj -c Release -o out /app/out/HappeningService

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-happeningservice
WORKDIR /app
COPY --from=build-happeningservice /app/out/HappeningService ./

# Expose port
EXPOSE 80 
ENTRYPOINT ["dotnet", "HappeningService.dll"]