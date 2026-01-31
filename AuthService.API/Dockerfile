# ----------------------------
# Build stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj(s) first for faster restore
COPY ["AuthService.API/AuthService.API.csproj", "AuthService.API/"]
COPY ["AuthService.Application/AuthService.Application.csproj", "AuthService.Application/"]
COPY ["AuthService.Infrastructure/AuthService.Infrastructure.csproj", "AuthService.Infrastructure/"]
COPY ["AuthService.Domain/AuthService.Domain.csproj", "AuthService.Domain/"]

RUN dotnet restore "AuthService.API/AuthService.API.csproj"

# Copy the rest
COPY . .

# Publish
RUN dotnet publish "AuthService.API/AuthService.API.csproj" -c Release -o /app/publish --no-restore

# ----------------------------
# Runtime stage
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AuthService.API.dll"]