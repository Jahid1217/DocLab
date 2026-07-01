# escape=`
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2022 AS build

WORKDIR /src
COPY . .

RUN nuget restore WebApplicationDocLab.sln
RUN msbuild WebApplicationDocLab.sln /p:Configuration=Release /p:DeployOnBuild=true /p:WebPublishMethod=FileSystem /p:DeleteExistingFiles=True /p:publishUrl=C:\publish

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2022

WORKDIR /inetpub/wwwroot
COPY --from=build C:\publish\ .

EXPOSE 80
