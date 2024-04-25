ARG FRAMEWORK_VERSION=6.0
ARG BUILD_CONFIGURATION=Release
FROM mcr.microsoft.com/dotnet/runtime:$FRAMEWORK_VERSION AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:$FRAMEWORK_VERSION AS build
ARG FRAMEWORK_VERSION
ARG BUILD_CONFIGURATION
WORKDIR /src
COPY . .
RUN dotnet build RedditTracker.sln --property:"TargetFrameworks=net$FRAMEWORK_VERSION" -c $BUILD_CONFIGURATION -o /src/dist

FROM build AS publish
ARG FRAMEWORK_VERSION
ARG BUILD_CONFIGURATION
RUN dotnet publish RedditTracker.sln --property:"TargetFrameworks=net$FRAMEWORK_VERSION" --framework net$FRAMEWORK_VERSION -c $BUILD_CONFIGURATION -o /src/public /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /src/public ./
ENTRYPOINT ["dotnet", "RedditTracker.dll"]
