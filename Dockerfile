FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/LuizaLabs.Challenge/LuizaLabs.Challenge.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "LuizaLabs.Challenge.dll"]

EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

EXPOSE 5001
ENV ASPNETCORE_URLS=https://*:5001