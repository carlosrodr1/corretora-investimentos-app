FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Investimentos.Producer/Investimentos.Producer.csproj ./Investimentos.Producer/
RUN dotnet restore ./Investimentos.Producer/Investimentos.Producer.csproj

COPY . .

WORKDIR /src/Investimentos.Producer
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && \
    apt-get install -y netcat-openbsd && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

COPY Investimentos.Producer/appsettings.Producer.json ./
COPY Investimentos.Producer/appsettings.Producer.Development.json ./

ENTRYPOINT ["dotnet", "Investimentos.Producer.dll"]
