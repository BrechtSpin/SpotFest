# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-userauthservice
WORKDIR /src

COPY UserAuthService/UserAuthService.csproj UserAuthService/
RUN dotnet restore UserAuthService/UserAuthService.csproj

COPY . .
RUN dotnet publish UserAuthService/UserAuthService.csproj -c Release -o /app/out/UserAuthService

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-userauthservice
WORKDIR /app
COPY --from=build-userauthservice /app/out/UserAuthService ./

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "UserAuthService.dll"]