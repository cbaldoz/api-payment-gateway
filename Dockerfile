# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY PaymongoApi/PayMongo.Payment.Api.csproj ./PayMongo.Payment.Api/
RUN dotnet restore ./PaymongoApi/PayMongo.Payment.Api.csproj

# Copy the rest and publish
COPY . ./
WORKDIR /app/PaymongoApi
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port (change if different)
EXPOSE 11000

# Set entry point
ENTRYPOINT ["dotnet", "PayMongo.Payment.Api.dll"]
