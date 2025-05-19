# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-informationservice
WORKDIR /app

COPY InformationService/InformationService.csproj InformationService/

RUN dotnet restore InformationService/InformationService.csproj
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-informationservice
WORKDIR /app
COPY --from=build-informationservice /app/out ./

# Expose the port and define the entry point
EXPOSE 80 5672
ENTRYPOINT ["dotnet", "InformationService.dll"]