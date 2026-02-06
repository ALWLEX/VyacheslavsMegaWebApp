FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY VyacheslavsMegaServer.csproj VyacheslavsMegaServer.csproj
RUN dotnet restore VyacheslavsMegaServer.csproj
COPY . .
WORKDIR /src
RUN dotnet build VyacheslavsMegaServer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VyacheslavsMegaServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VyacheslavsMegaServer.dll"]