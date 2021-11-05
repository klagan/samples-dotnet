FROM ghcr.io/klagan/net5:1.0 AS BUILDER
ARG DOTNET_SOLUTION
COPY . /mysource
RUN  sudo mkdir /output
WORKDIR /mysource

RUN if [ -f "$DOTNET_SOLUTION" ]; then sudo dotnet publish --configuration Release --output /output "$DOTNET_SOLUTION"; else echo "Solution not found" ; fi

#==========================================================================================================================
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS RUNNER
RUN groupadd -r basic && useradd --no-log-init -r -g basic apprunner
USER apprunner
COPY --chown=apprunner:basic --from=BUILDER /output /app

ENV ASPNETCORE_URLS=http://+:5000 \
DOTNET_USE_POLLING_FILE_WATCHER=true \
ASPNETCORE_ENVIRONMENT=Development 

EXPOSE 5001
EXPOSE 5000

ENTRYPOINT ["dotnet", "/app/Samples.WebApi.dll"]
