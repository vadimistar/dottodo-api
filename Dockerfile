FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY dottodo/dottodo.csproj .
RUN dotnet restore

COPY dottodo/. .
RUN dotnet publish --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 5072
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./dottodo"]