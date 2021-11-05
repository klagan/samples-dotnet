# Getting started

Remote containers are a nice way of isolating environments for development.  They allow a developer to run inside a pre-configured container which is detailed in a manifest (dockerfile).  This can be taken a step further by having the containers activate on remote servers over SSH.  This means that the source code doesn't even touch your machine but is instead being **puppeted** on a remote server in a remote container.

Taking this a step further we can reuse the same container to build locally and in the CI/CD pipeline and thereby offering full transparency and autonomy to a developer in configuring builds.

---

## 1. Create a `devcontainer`

`Visual Studio Code` makes this simple by providing a step by step guide on which type of container image you would like the development environment to encapsulate.  This example uses `net 5.0`.

We can do this via the `vscode` `command palette`.

[Microsoft instructions](https://code.visualstudio.com/docs/remote/create-dev-container)

## 2. Customise the `Dockerfile`

The `devcontainer` consists of two parts: Dockerfile and remote container configuration.  The `Dockerfile` is a standard file which we may edit as we wish.

## 3. Pulling in credentials (e.g. nuget)

Now we are able to open a remote container in `vscode` that is configured to build and develop `net 5.0` applications.  One issue I ran into was managing nuget credentials.  When running, the container is a transient entity and does not persist changes.  If we want changes to persist we may alter the original `Dockerfile` or we can use `docker bind mounts`.

In this instance, we do not want to persist our nuget credentials in the image as this would be visible to all who have access to the repository we store the `Dockerfile`.  Instead, we can have the docker image use the host credentials.  The host in this context is the machine the remote container is running - in this case my laptop.

My laptop is considered **safe** and the containers only have access to that which I share with it.  We shall mount our host nuget configuration folder - on an Apple Mac this is `~/.nuget` - into the container and map it to the container nuget folder.

We do this by adding the following section into the `devcontainer.json` we created in the first step:

```docker
    "mounts": [
        "source=${env:HOME}${env:USERPROFILE}/.nuget,target=/home/vscode/.nuget,type=bind"
    ],
```

The sample above tells the configuration to map my host nuget folder to the same folder in the container - (vscode is the user in the container).

This allows us to use private nuget sources by adding them to the `nuget.config` file either in the mapped nuget configuration location or directly in the solution as a `nuget.config` file.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="My Private NuGet Source" value="https://pkgs.dev.azure.com/my-private-nuget-source/nuget/v3/index.json" />
  </packageSources>
  <packageRestore>
    <add key="enabled" value="true" />
    <add key="automatic" value="true" />
  </packageRestore>
</configuration>
```

Now we may add, restore and remove packages to the project.  We may need to authenticate to generate an authorisation token but now they will persist on the host machine which the container accesses.

```dotnetcli
# add a package from a private nuget source
dotnet add package My.Private.Package --interactive      

# restore packages from private nuget sources
dotnet restore --interactive
```

## 4. Create a `docker-compose` file too

I also add a `docker-compose` file to mirror in the terminal what a `devcontainer` does in the IDE allowing for ad-hoc access from the terminal.

> I see there is provision for using `docker-compose` files as a `devcontainer`, but I haven't figured out how this works quite yet, so TBD!
> Note to self: remember to define the `user` in the `Dockerfile` and/or `docker-compose` else the container will not function correctly.

To run the `docker-compose` container we run:

```dotnetcli
docker-compose run <serviceName> <shell>

# e.g. docker-compose run builder pwsh
# e.g. docker-compose run builder zsh
```

Once the prompt appears, you are located in the root of the filesystem and the source code - located on the host - is in the `/src` folder.  Below is an example of starting the container, navigating to the root source folder and building the solution:

```bash
klagan@ubuntu  ~/source/github/samples-dotnet/src/devcontainers   master ±  docker-compose run environment
vscode ➜ / $ ls
bin  boot  dev  etc  home  lib  lib64  media  mnt  opt  proc  root  run  sbin  source  srv  sys  tmp  usr  var  vscode
vscode ➜ / $ cd source
vscode ➜ /source $ ls
README.md  Samples.Sample.sln  build  docker-compose.yml  src
vscode ➜ /source $ dotnet build Samples.Sample.sln

Welcome to .NET 5.0!
---------------------
SDK Version: 5.0.402

Telemetry
---------
The .NET tools collect usage data in order to help us improve your experience. It is collected by Microsoft and shared with the community. You can opt-out of telemetry by setting the DOTNET_CLI_TELEMETRY_OPTOUT environment variable to '1' or 'true' using your favourite shell.

Read more about .NET CLI Tools telemetry: https://aka.ms/dotnet-cli-telemetry

----------------
Installed an ASP.NET Core HTTPS development certificate.
To trust the certificate run 'dotnet dev-certs https --trust' (Windows and macOS only).
Learn about HTTPS: https://aka.ms/dotnet-https
----------------
Write your first app: https://aka.ms/dotnet-hello-world
Find out what's new: https://aka.ms/dotnet-whats-new
Explore documentation: https://aka.ms/dotnet-docs
Report issues and find source on GitHub: https://github.com/dotnet/core
Use 'dotnet --help' to see available commands or visit: https://aka.ms/dotnet-cli
--------------------------------------------------------------------------------------
Microsoft (R) Build Engine version 16.11.1+3e40a09f8 for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  Restored /source/src/Samples.WebApi/Samples.WebApi.csproj (in 146 ms).
  Restored /source/src/Samples.Bll/Samples.Bll.csproj (in 146 ms).
  Samples.WebApi -> /source/src/Samples.WebApi/bin/Debug/net5.0/Samples.WebApi.dll
  Samples.Bll -> /source/src/Samples.Bll/bin/Debug/net5.0/Samples.Bll.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:02.39
vscode ➜ /source $ 
```

There is also a `docker-compose` service for `runner` which will build, pack and run the application.  The main difference with the `runner` service and the `environment` service is in the `build` section where we now include a solution file:

```yaml
    build:
      context: .
      dockerfile: build/Dockerfile
      target: RUNNER
      args: 
        DOTNET_SOLUTION: Samples.Sample.sln
```

The referenced `Dockerfile` is a reusable construct accepts the solution parameter for action on the generic `dotnet publish` command.

The aim is to guide the developer to keep things simple by managing the code through a solution file which keeping the generic reuse of the underlying docker framework intact.

The following command will build, package the image and run the application:

```dotnetcli
docker-compose up runner
```

When up you may check it is running by opening a browser and navigating to `http://localhost:5000/weatherforecast`
