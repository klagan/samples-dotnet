FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build

# copy the source files
COPY . /build/src

WORKDIR /build/src/MyWebAPI

# add the dotnet swagger tool
# allows openapi file extraction 
RUN dotnet new tool-manifest \
    && dotnet tool install --version 6.1.4 Swashbuckle.AspNetCore.Cli \
    && dotnet tool list

WORKDIR /build

# publish output (restore, build and export)
RUN dotnet publish \
  # --runtime alpine-x64 \
  # --self-contained true \
  # /p:PublishTrimmed=true \
  # /p:PublishSingleFile=true \
  -c Release \
  # --configfile nuget.config \
  -o ./output \
  src/MyWebAPI/MyWebAPI.csproj

WORKDIR /build/src/MyWebAPI

RUN mkdir -p /build/output/wwwroot/api-docs/v1/ \
    && dotnet swagger tofile --output /build/output/wwwroot/api-docs/v1/openapi.yml /build/output/MyWebAPI.dll v1 


FROM node:latest as swagger_build

RUN apt-get update && \
    apt-get upgrade -y

COPY --from=build /build/output/ .

RUN npx redoc-cli bundle --output ./wwwroot/api-docs/v1/openapi.htm --title "Version 1.0.0"  --options.theme.colors.primary.main=blue ./wwwroot/api-docs/v1/openapi.yml



FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.10-bionic AS final

RUN apt-get update && \
    apt-get install -y --no-install-recommends && \
    apt-get install -y apache2-utils && \
    apt-get install -y libgif-dev autoconf libtool automake build-essential gettext libglib2.0-dev libcairo2-dev libtiff-dev libexif-dev libgdiplus && \
    apt upgrade -y

RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app

COPY --chown=app --from=swagger_build / /app

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000 \
DOTNET_USE_POLLING_FILE_WATCHER=true \
ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "MyWebAPI.dll"]



# FROM scratch AS export
# COPY --from=build /build/output/authorisation_tests.trx /


