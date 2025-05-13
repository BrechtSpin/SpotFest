# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dataharvester
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-dataharvester
WORKDIR /app
COPY --from=build-dataharvester /app/out ./

# Expose the port and define the entry point
EXPOSE 80, 5672
ENTRYPOINT ["dotnet", "DataHarvester.dll"]