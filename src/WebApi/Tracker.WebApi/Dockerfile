#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/WebApi/Tracker.WebApi/Tracker.WebApi.csproj", "src/WebApi/Tracker.WebApi/"]
COPY ["src/Application/Tracker.Application/Tracker.Application.csproj", "src/Application/Tracker.Application/"]
COPY ["src/Core/Tracker.Core/Tracker.Core.csproj", "src/Core/Tracker.Core/"]
COPY ["src/Shared/Tracker.Shared/Tracker.Shared.csproj", "src/Shared/Tracker.Shared/"]
COPY ["src/Infrastructure/Tracker.Infrastructure/Tracker.Infrastructure.csproj", "src/Infrastructure/Tracker.Infrastructure/"]
RUN dotnet restore "src/WebApi/Tracker.WebApi/Tracker.WebApi.csproj"
COPY . .
WORKDIR "/src/src/WebApi/Tracker.WebApi"
RUN dotnet build "Tracker.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tracker.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tracker.WebApi.dll"]