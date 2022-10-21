FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloWorldAPI.csproj", "."]
RUN dotnet restore "./HelloWorldAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "HelloWorldAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloWorldAPI.csproj" -c Release -r linux-musl-x64 --no-self-contained -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloWorldAPI.dll"]

HEALTHCHECK CMD curl -f http://localhost/live || exit 1
