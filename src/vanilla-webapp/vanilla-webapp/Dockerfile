FROM mcr.microsoft.com/dotnet/sdk:3.1 AS builder
WORKDIR /source

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish --output /app/ --configuration Release

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=builder /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "vanilla-webapp.dll"]