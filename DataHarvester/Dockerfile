# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dataharvester
WORKDIR /src

COPY DataHarvester/DataHarvester.csproj DataHarvester/
RUN dotnet restore DataHarvester/DataHarvester.csproj

COPY Contracts/Contracts.csproj Contracts/

COPY . .
RUN dotnet publish DataHarvester/DataHarvester.csproj -c Release -o /app/out/DataHarvester

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-dataharvester
WORKDIR /app
COPY --from=build-dataharvester /app/out/HappeningService ./

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "DataHarvester.dll"]