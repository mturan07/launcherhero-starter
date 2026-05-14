# ── Stage 1: build & publish ──────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY ["LauncherHero.Starter.csproj", "."]
RUN dotnet restore "LauncherHero.Starter.csproj"

COPY . .
RUN dotnet publish "LauncherHero.Starter.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ── Stage 2: final runtime ────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

RUN addgroup -S appgroup \
 && adduser -S appuser -G appgroup

COPY --from=build --chown=appuser:appgroup /app/publish .

USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "LauncherHero.Starter.dll"]
