#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lancamento.API/Lancamento.API.csproj", "Lancamento.API/"]
COPY ["Lancamento.API.Application/Lancamento.API.Application.csproj", "Lancamento.API.Application/"]
COPY ["Lancamento.API.Domain/Lancamento.API.Domain.csproj", "Lancamento.API.Domain/"]
COPY ["Lancamento.API.Infra.Data/Lancamento.API.Infra.Data.csproj", "Lancamento.API.Infra.Data/"]
RUN dotnet restore "./Lancamento.API/./Lancamento.API.csproj"
COPY . .
WORKDIR "/src/Lancamento.API"
RUN dotnet build "./Lancamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Lancamento.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lancamento.API.dll"]