# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CoreDisplay.Api/CoreDisplay.Api.csproj", "CoreDisplay.Api/"]
COPY ["CoreDisplay.Application/CoreDisplay.Application.csproj", "CoreDisplay.Application/"]
COPY ["CoreDisplay.Domain/CoreDisplay.Domain.csproj", "CoreDisplay.Domain/"]
COPY ["CoreDisplay.Infrastructure/CoreDisplay.Infrastructure.csproj", "CoreDisplay.Infrastructure/"]
COPY ["CoreDisplay.Shared/CoreDisplay.Shared.csproj", "CoreDisplay.Shared/"]
RUN dotnet restore "CoreDisplay.Api/CoreDisplay.Api.csproj"
COPY . .
WORKDIR "/src/CoreDisplay.Api"
RUN dotnet build "CoreDisplay.Api.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "CoreDisplay.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CoreDisplay.Api.dll"]
