FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY VyacheslavsMegaWebApp.csproj VyacheslavsMegaWebApp.csproj
RUN dotnet restore VyacheslavsMegaWebApp.csproj
COPY . .
WORKDIR /src
RUN dotnet build VyacheslavsMegaWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VyacheslavsMegaWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VyacheslavsMegaWebApp.dll"]