FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Investimentos.Api/*.csproj ./Investimentos.Api/
RUN dotnet restore ./Investimentos.Api/Investimentos.Api.csproj

COPY . .

WORKDIR /src/Investimentos.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Investimentos.Api.dll"]
