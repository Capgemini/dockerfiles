$ErrorActionPreference = 'Stop';

Write-Host Starting build stage - net-build...

# build base imageo
docker build -t $env:DOCKER_REGISTRY/net-build -f net-build/Dockerfile .

# image list
docker images

if($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode )  }