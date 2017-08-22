$ErrorActionPreference = 'Stop';

if (! (Test-Path Env:\APPVEYOR_REPO_TAG_NAME)) {
  Write-Host "No version tag detected. Skip publishing."
  exit 0
}

Write-Host Starting push stage - net-build...

# invoke the build for container re-build
./build.ps1

# log on to docker
docker login $env:DOCKER_REGISTRY -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"

# tag with the build number
docker tag idevcr.azurecr.io/net-build idevcr.azurecr.io/net-build:$env:APPVEYOR_REPO_TAG_NAME

# push to registry 
docker push idevcr.azurecr.io/net-build:$env:APPVEYOR_REPO_TAG_NAME