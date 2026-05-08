# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files for restore
COPY Mroz.sln .
COPY global.json .
COPY Api/Api.csproj Api/
COPY BusinessLogic/BusinessLogic.csproj BusinessLogic/
COPY DataAccess/DataAccess.csproj DataAccess/
COPY Domain/Domain.csproj Domain/

# Restore entire solution so all projects and refs are resolved
RUN dotnet restore Mroz.sln

# Copy remaining source and publish
COPY . .
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# PORT is set by Railway at runtime; Program.cs binds to 0.0.0.0:PORT
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Mroz.dll"]
