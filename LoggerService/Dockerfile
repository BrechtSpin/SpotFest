# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-loggerservice
WORKDIR /src

COPY LoggerService/LoggerService.csproj LoggerService/
RUN dotnet restore LoggerService/LoggerService.csproj

COPY Contracts/Contracts.csproj Contracts/

COPY . .
RUN dotnet publish LoggerService/LoggerService.csproj -c Release -o /app/out/LoggerService

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-loggerservice
WORKDIR /app
COPY --from=build-loggerservice /app/out/LoggerService ./

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "LoggerService.dll"]