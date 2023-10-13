#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# --- NPM tools are grabbed here ---
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install -y nodejs

# --- Back to normal steps ---
COPY ["SPAService/SPAService.csproj", "SPAService/"]
RUN dotnet restore "SPAService/SPAService.csproj"
COPY . .

# --- After moving to the right directory, grab the necessary packages from npm and build the needed files ---
WORKDIR "/src/SPAService/ClientApp"
RUN npm install
WORKDIR "/src/SPAService"
RUN dotnet build "SPAService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SPAService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPAService.dll"]