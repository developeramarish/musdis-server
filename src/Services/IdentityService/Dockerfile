# Run from the root of the Musdis project (musdis-server/)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY ./packages ./packages

WORKDIR /source
COPY ./src/Services/IdentityService ./
RUN dotnet restore "./IdentityService.csproj" -s ../packages -s https://api.nuget.org/v3/index.json
RUN dotnet publish "./IdentityService.csproj" -o /app -c release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
WORKDIR /app
COPY --from=build /app ./

EXPOSE 7002

ENTRYPOINT ["dotnet", "IdentityService.dll"]

