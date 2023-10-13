#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BusStaffService/BusStaffService.csproj", "BusStaffService/"]
COPY ["BarebonesRabbitMQImplementationLibrary/*.csproj", "BarebonesRabbitMQImplementationLibrary/"]
RUN dotnet restore "BusStaffService/BusStaffService.csproj"
COPY . .
WORKDIR "/src/BusStaffService"
RUN dotnet build "BusStaffService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusStaffService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusStaffService.dll"]