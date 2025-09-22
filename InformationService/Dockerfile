# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-informationservice
WORKDIR /src

COPY InformationService/InformationService.csproj InformationService/
RUN dotnet restore InformationService/InformationService.csproj

COPY . .
RUN dotnet publish InformationService/InformationService.csproj -c Release -o /app/out/InformationService

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-informationservice
WORKDIR /app
COPY --from=build-informationservice /app/out/InformationService ./

# Expose the port and define the entry point
EXPOSE 80
ENTRYPOINT ["dotnet", "InformationService.dll"]