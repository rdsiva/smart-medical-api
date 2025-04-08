# API Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SmartMedical.API/SmartMedical.API.csproj", "SmartMedical.API/"]
COPY ["SmartMedical.Core/SmartMedical.Core.csproj", "SmartMedical.Core/"]
COPY ["SmartMedical.Infrastructure/SmartMedical.Infrastructure.csproj", "SmartMedical.Infrastructure/"]
RUN dotnet restore "SmartMedical.API/SmartMedical.API.csproj"
COPY . .
WORKDIR "/src/SmartMedical.API"
RUN dotnet build "SmartMedical.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartMedical.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartMedical.API.dll"]
