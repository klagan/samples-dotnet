FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /src
COPY ["MyWebAPI/MyWebAPI.csproj", "MyWebAPI/"]
COPY ["MyWeather.Models/MyWeather.Models.csproj", "MyWeather.Models/"]
COPY ["MyWeather.Bll/MyWeather.Bll.csproj", "MyWeather.Bll/"]
COPY ["MyUserName.Bll/MyUserName.Bll.csproj", "MyUserName.Bll/"]
COPY ["OpenAPI.sln", "./"]
RUN dotnet restore "/src/OpenAPI.sln"
COPY . .
RUN dotnet build "/src/OpenAPI.sln" -c Release -o /app/build


FROM build AS publish
WORKDIR /app/build

# add the dotnet swagger tool
# allows openapi file extraction 
RUN dotnet new tool-manifest \
    && dotnet tool install --version 6.1.4 Swashbuckle.AspNetCore.Cli \
    && dotnet tool list 

# publish output (restore, build and export)
RUN dotnet publish \
  # --runtime alpine-x64 \
  # --self-contained true \
  # /p:PublishTrimmed=true \
  # /p:PublishSingleFile=true \
  -c Release \
  # --configfile nuget.config \
  -o /app/publish \
  /src/OpenAPI.sln

RUN mkdir -p /app/publish/wwwroot/api-docs/v1/ 
RUN cat .config/dotnet-tools.json   
RUN dotnet swagger tofile --output /app/publish/wwwroot/api-docs/v1/openapi.yml /app/publish/MyWebAPI.dll v1 
RUN apt-get update && apt-get install -y tree && tree

##################################################################################################

FROM node:latest as swagger_build
RUN apt-get update && \
    apt-get upgrade -y

COPY --from=publish /app/publish/ /output
RUN npx redoc-cli bundle --output /output/wwwroot/api-docs/v1/openapi.htm --title "Version 1.0.0"  --options.theme.colors.primary.main=blue /output/wwwroot/api-docs/v1/openapi.yml

##################################################################################################

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.10-bionic AS final

# RUN apt-get update && \
#     apt-get install -y --no-install-recommends && \
#     apt-get install -y apache2-utils && \
#     apt-get install -y libgif-dev autoconf libtool automake build-essential gettext libglib2.0-dev libcairo2-dev libtiff-dev libexif-dev libgdiplus && \
#     apt upgrade -y

ARG PORT=5000

RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app

COPY --chown=app --from=swagger_build /output /app

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000 

EXPOSE 5000

ENTRYPOINT ["dotnet", "MyWebAPI.dll"]

##################################################################################################

# FROM scratch AS export
# COPY --from=build /build/output/authorisation_tests.trx /


