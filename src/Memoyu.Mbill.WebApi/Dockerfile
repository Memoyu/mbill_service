#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Memoyu.Mbill.WebApi/Memoyu.Mbill.WebApi.csproj", "src/Memoyu.Mbill.WebApi/"]
COPY ["src/Memoyu.Mbill.Application/Memoyu.Mbill.Application.csproj", "src/Memoyu.Mbill.Application/"]
COPY ["src/Memoyu.Mbill.Application.Contracts/Memoyu.Mbill.Application.Contracts.csproj", "src/Memoyu.Mbill.Application.Contracts/"]
COPY ["src/Memoyu.Mbill.Domain/Memoyu.Mbill.Domain.csproj", "src/Memoyu.Mbill.Domain/"]
COPY ["src/Memoyu.Mbill.Domain.Shared/Memoyu.Mbill.Domain.Shared.csproj", "src/Memoyu.Mbill.Domain.Shared/"]
COPY ["src/Memoyu.Mbill.ToolKits/Memoyu.Mbill.ToolKits.csproj", "src/Memoyu.Mbill.ToolKits/"]
COPY ["src/Memoyu.Mbill.BackgroundJobs/Memoyu.Mbill.BackgroundJobs.csproj", "src/Memoyu.Mbill.BackgroundJobs/"]
RUN dotnet restore "src/Memoyu.Mbill.WebApi/Memoyu.Mbill.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Memoyu.Mbill.WebApi"
RUN dotnet build "Memoyu.Mbill.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Memoyu.Mbill.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Memoyu.Mbill.WebApi.dll"]