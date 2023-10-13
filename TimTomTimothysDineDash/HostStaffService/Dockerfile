#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HostStaffService/HostStaffService.csproj", "HostStaffService/"]
COPY ["BarebonesRabbitMQImplementationLibrary/*.csproj", "BarebonesRabbitMQImplementationLibrary/"]
RUN dotnet restore "HostStaffService/HostStaffService.csproj"
COPY . .
WORKDIR "/src/HostStaffService"
RUN dotnet build "HostStaffService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HostStaffService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HostStaffService.dll"]