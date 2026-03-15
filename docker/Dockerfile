# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia arquivos de projeto primeiro para aproveitar cache
COPY IdentityCampaign.sln ./
COPY src/IdentityCampaign.Api/IdentityCampaign.Api.csproj src/IdentityCampaign.Api/
COPY src/IdentityCampaign.Application/IdentityCampaign.Application.csproj src/IdentityCampaign.Application/
COPY src/IdentityCampaign.Domain/IdentityCampaign.Domain.csproj src/IdentityCampaign.Domain/
COPY src/IdentityCampaign.Infrastructure/IdentityCampaign.Infrastructure.csproj src/IdentityCampaign.Infrastructure/

RUN dotnet restore IdentityCampaign.sln

# Copia o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish src/IdentityCampaign.Api/IdentityCampaign.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IdentityCampaign.Api.dll"]