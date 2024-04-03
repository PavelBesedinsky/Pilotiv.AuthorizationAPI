FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj", "Pilotiv.AuthorizationAPI.WebUI/"]
COPY ["Pilotiv.AuthorizationAPI.Application/Pilotiv.AuthorizationAPI.Application.csproj", "Pilotiv.AuthorizationAPI.Application/"]
COPY ["Pilotiv.AuthorizationAPI.Domain/Pilotiv.AuthorizationAPI.Domain.csproj", "Pilotiv.AuthorizationAPI.Domain/"]
COPY ["Pilotiv.AuthorizationAPI.Infrastructure/Pilotiv.AuthorizationAPI.Infrastructure.csproj", "Pilotiv.AuthorizationAPI.Infrastructure/"]
ARG BUILD_CONFIGURATION=Release
RUN dotnet restore "Pilotiv.AuthorizationAPI.WebUI/Pilotiv.AuthorizationAPI.WebUI.csproj"
COPY . .
WORKDIR "/src/Pilotiv.AuthorizationAPI.WebUI"
RUN dotnet build "Pilotiv.AuthorizationAPI.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Pilotiv.AuthorizationAPI.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pilotiv.AuthorizationAPI.WebUI.dll"]
