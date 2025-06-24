# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file first using correct path
COPY PaymongoApi/PayMongo.Payment.Api.csproj ./PaymongoApi/
RUN dotnet restore ./PaymongoApi/PayMongo.Payment.Api.csproj

# Copy the rest of the code
COPY . ./
WORKDIR /app/PaymongoApi
RUN dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "PayMongo.Payment.Api.dll"]
