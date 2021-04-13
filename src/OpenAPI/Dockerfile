FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build

# copy the source files
COPY . /build/src

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

COPY --chown=app --from=build /build/output/ /app

WORKDIR /app

ENV ASPNETCORE_URLS= \
DOTNET_USE_POLLING_FILE_WATCHER=true \
ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "MyWebAPI.dll"]



# FROM scratch AS export
# COPY --from=build /build/output/authorisation_tests.trx /


