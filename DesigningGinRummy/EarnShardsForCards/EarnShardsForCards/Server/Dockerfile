#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EarnShardsForCards/Server/EarnShardsForCards.Server.csproj", "EarnShardsForCards/Server/"]
COPY ["EarnShardsForCards/Client/EarnShardsForCards.Client.csproj", "EarnShardsForCards/Client/"]
COPY ["EarnShardsForCards/Shared/EarnShardsForCards.Shared.csproj", "EarnShardsForCards/Shared/"]
RUN dotnet restore "EarnShardsForCards/Server/EarnShardsForCards.Server.csproj"
COPY . .
WORKDIR "/src/EarnShardsForCards/Server"
RUN dotnet build "EarnShardsForCards.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EarnShardsForCards.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EarnShardsForCards.Server.dll"]