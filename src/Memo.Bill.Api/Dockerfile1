#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Memo.Bill.Api/Memo.Bill.Api.csproj", "src/Memo.Bill.Api/"]
COPY ["src/Memo.Bill.Application/Memo.Bill.Application.csproj", "src/Memo.Bill.Application/"]
COPY ["src/Memo.Bill.Domain/Memo.Bill.Domain.csproj", "src/Memo.Bill.Domain/"]
COPY ["src/Memo.Bill.Infrastructure/Memo.Bill.Infrastructure.csproj", "src/Memo.Bill.Infrastructure/"]
RUN dotnet restore "./src/Memo.Bill.Api/Memo.Bill.Api.csproj"
COPY . .
WORKDIR "/src/src/Memo.Bill.Api"
RUN dotnet build "./Memo.Bill.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Memo.Bill.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Memo.Bill.Api.dll"]