$ErrorActionPreference = 'Stop';

Write-Host Starting build stage - net-build...

# pull latest images
docker pull microsoft/windowsservercore

# build base image
docker build -t $env:DOCKER_REGISTRY/net-build -f net-build/Dockerfile .

if($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode )  }

# image list
docker images