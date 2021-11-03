# Image that is required from docker hub to start the build this will be used to build our project
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# set the working directory
WORKDIR /app

# copy csproj file (from host to inside container) to know dependency and restore it.
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build it.
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
