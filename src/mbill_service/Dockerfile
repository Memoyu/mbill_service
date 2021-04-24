#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/mbill_service/mbill_service.csproj", "src/mbill_service/"]
COPY ["src/mbill_service.Service/mbill_service.Service.csproj", "src/mbill_service.Service/"]
COPY ["src/mbill_service.Core/mbill_service.Core.csproj", "src/mbill_service.Core/"]
COPY ["src/mbill_service.ToolKits/mbill_service.ToolKits.csproj", "src/mbill_service.ToolKits/"]
COPY ["src/mbill_service.Infrastructure/mbill_service.Infrastructure.csproj", "src/mbill_service.Infrastructure/"]
RUN dotnet restore "src/mbill_service/mbill_service.csproj"
COPY . .
WORKDIR "/src/src/mbill_service"
RUN dotnet build "mbill_service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mbill_service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mbill_service.dll"]