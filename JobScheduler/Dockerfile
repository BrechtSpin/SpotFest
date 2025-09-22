# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-jobscheduler
WORKDIR /src

COPY JobScheduler/JobScheduler.csproj JobScheduler/
RUN dotnet restore JobScheduler/JobScheduler.csproj

COPY Contracts/Contracts.csproj Contracts/

COPY . .
RUN dotnet publish JobScheduler/JobScheduler.csproj -c Release -o /app/out/JobScheduler

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-jobscheduler
WORKDIR /app
COPY --from=build-jobscheduler /app/out/JobScheduler ./

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "JobScheduler.dll"]