# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-jobscheduler
WORKDIR /app

COPY JobScheduler/JobScheduler.csproj JobScheduler/
COPY Contracts/Contracts.csproj Contracts/

RUN dotnet restore JobScheduler/JobScheduler.csproj
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-jobscheduler
WORKDIR /app
COPY --from=build-jobscheduler /app/out ./

# Expose the port and define the entry point
EXPOSE 80 5672
ENTRYPOINT ["dotnet", "JobScheduler.dll"]