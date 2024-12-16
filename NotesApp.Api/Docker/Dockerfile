# Use the official .NET SDK image to build and publish the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NotesApp.Api/NotesApp.Api.csproj", "NotesApp.Api/"]
WORKDIR /src/NotesApp.Api
RUN dotnet restore

# Copy all project files and restore dependencies
COPY NotesApp.Api/ .

# Build the API project
RUN dotnet build -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Set environment variable to ensure correct port binding
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "NotesApp.Api.dll"]