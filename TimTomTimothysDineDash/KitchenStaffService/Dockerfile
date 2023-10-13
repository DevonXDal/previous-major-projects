#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KitchenStaffService/KitchenStaffService.csproj", "KitchenStaffService/"]
COPY ["BarebonesRabbitMQImplementationLibrary/*.csproj", "BarebonesRabbitMQImplementationLibrary/"]
RUN dotnet restore "KitchenStaffService/KitchenStaffService.csproj"
COPY . .
WORKDIR "/src/KitchenStaffService"
RUN dotnet build "KitchenStaffService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KitchenStaffService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KitchenStaffService.dll"]