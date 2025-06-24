# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy everything and restore
COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Use runtime image for final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set working directory for runtime
WORKDIR /app

# Copy from build image
COPY --from=build /app/out .

# Expose the port your app listens on (default for Web API is 80)
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "YourAppName.dll"]
