FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Copy everything else and build
COPY LeagueBalancer ./LeagueBalancer

RUN dotnet restore LeagueBalancer/LeagueBalancer.csproj
RUN dotnet publish LeagueBalancer/*.csproj -c Release -o out

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env ./out .
ENTRYPOINT ["dotnet", "LeagueBalancer.dll"]