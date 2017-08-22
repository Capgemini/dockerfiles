$ErrorActionPreference = 'Stop';

Write-Host Starting build stage - net-build...

# build the base image
docker build -t idevcr.azurecr.io/net-build -f Dockerfile .


if($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode )  }