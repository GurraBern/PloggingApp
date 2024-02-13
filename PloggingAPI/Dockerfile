FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS build-env
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PloggingAPI/PloggingAPI.csproj", "PloggingAPI/"]
RUN dotnet restore "./PloggingAPI/./PloggingAPI.csproj"
COPY . .
WORKDIR "/src/PloggingAPI"
RUN dotnet build "./PloggingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PloggingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PloggingAPI.dll"]



# Use the Microsoft .NET Core SDK image as the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy only the necessary project files and restore as distinct layers
COPY Plogging.Core/Plogging.Core.csproj Plogging.Core/
COPY PloggingAPI/PloggingAPI.csproj PloggingAPI/
RUN dotnet restore PloggingAPI/PloggingAPI.csproj

# Copy the entire project source code
COPY . .

# Build the PloggingAPI project without building PloggingApp
WORKDIR /app/PloggingAPI
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/PloggingAPI/out .
ENTRYPOINT ["dotnet", "PloggingAPI.dll"]