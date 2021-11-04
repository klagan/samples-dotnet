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

I also add a `docker-compose` file to mirror in the terminal what a `devcontainer` does in the IDE.  The idea is, that going forward I will be able to add new sections to the `docker-compose` file that will include the ability to run the service.  I also see there is provision for using `docker-compose` files as a `devcontainer`, but I haven't figured out how this works quite yet, so TBD!

> Note to self: remember to define the `user` in the `Dockerfile` and/or `docker-compose` else the container will not function correctly.

To run the `docker-compose` container we run:

```dotnetcli
docker run <serviceName> <shell>

# e.g. docker run builder pwsh
# e.g. docker run builder zsh
```
