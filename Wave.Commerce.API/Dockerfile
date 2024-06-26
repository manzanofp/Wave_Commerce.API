#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wave.Commerce.API/*.csproj", "Wave.Commerce.API/"]
COPY ["Wave.Commerce.DependencyInjection/*.csproj", "Wave.Commerce.DependencyInjection/"]
COPY ["Wave.Commerce.Application/*.csproj", "Wave.Commerce.Application/"]
COPY ["Wave.Commerce.Domain/*.csproj", "Wave.Commerce.Domain/"]
COPY ["Wave.Commerce.Persistence/*.csproj", "Wave.Commerce.Persistence/"]
RUN dotnet restore "./Wave.Commerce.API/./Wave.Commerce.API.csproj"
COPY . .
WORKDIR "/src/Wave.Commerce.API"
RUN dotnet build "./Wave.Commerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wave.Commerce.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENV DATABASE_CONNECTION_STRING="Server=localhost;Database=WaveCommerce; UID=postgres; PWD=123456; Include Error Detail=true"
ENTRYPOINT ["dotnet", "Wave.Commerce.API.dll"]