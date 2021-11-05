# TODO: replace with a single stored image - commit this first image to a repository to be shared between here and the .devcontainer

# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.202.5/containers/dotnet/.devcontainer/base.Dockerfile

# [Choice] .NET version: 6.0, 5.0, 3.1, 6.0-bullseye, 5.0-bullseye, 3.1-bullseye, 6.0-focal, 5.0-focal, 3.1-focal
ARG VARIANT="5.0-bullseye-slim"
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT} AS ENVIRONMENT

# [Choice] Node.js version: none, lts/*, 16, 14, 12, 10
ARG NODE_VERSION="none"
RUN if [ "${NODE_VERSION}" != "none" ]; then su vscode -c "umask 0002 && . /usr/local/share/nvm/nvm.sh && nvm install ${NODE_VERSION} 2>&1"; fi

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1

# add .dotnet tools to path
ENV PATH /root/.dotnet/tools:$PATH

# default yes to prompts
RUN echo "APT::Get::Assume-Yes \"true\";" > /etc/apt/apt.conf.d/90assumeyes

# basic requirements
RUN apt-get update \
   && apt-get install -y --no-install-recommends \
   unzip \
   wget \
   nodejs \
   npm \
   vim

# add sonarscanner
RUN   apt update && apt install default-jre \
      && dotnet tool install --global dotnet-sonarscanner 

ARG   PWSHHOME="/home/vscode"
ARG   PWSHPROFILE="${PWSHHOME}/.config/powershell/Microsoft.PowerShell_profile.ps1"
ARG   PWSHUSER="vscode"

# add ohmyposh
RUN   sudo wget https://github.com/JanDeDobbeleer/oh-my-posh/releases/latest/download/posh-linux-amd64 -O /usr/local/bin/oh-my-posh \
      && sudo chmod +x /usr/local/bin/oh-my-posh \
      && sudo mkdir "${PWSHHOME}/.poshthemes" \
      && sudo wget https://github.com/JanDeDobbeleer/oh-my-posh/releases/latest/download/themes.zip -O "${PWSHHOME}/.poshthemes/themes.zip" \
      && sudo unzip "${PWSHHOME}/.poshthemes/themes.zip" -d "${PWSHHOME}/.poshthemes" \
      && sudo chmod 777 ${PWSHHOME}/.poshthemes/*.json \
      && sudo rm -f ${PWSHHOME}.poshthemes/themes.zip \
      && mkdir -p ${PWSHHOME}/.config/powershell/ ${PWSHUSER} \
      && echo "oh-my-posh --init --shell pwsh --config ${PWSHHOME}/.poshthemes/jandedobbeleer.omp.json | Invoke-Expression" > ${PWSHPROFILE}

#==========================================================================================================================
FROM ENVIRONMENT AS BUILDER
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
