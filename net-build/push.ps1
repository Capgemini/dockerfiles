$ErrorActionPreference = 'Stop';

if (! (Test-Path Env:\APPVEYOR_REPO_TAG_NAME)) {
  Write-Host "No version tag detected. Skip publishing."
  exit 0
}

Write-Host Starting push stage - net-build...

# log on
docker login -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"

# tag with the build number
docker tag $env:DOCKER_REGISTRY/net-build $env:DOCKER_REGISTRY/net-build:$env:APPVEYOR_REPO_TAG_NAME

# push to registry 
docker push $env:DOCKER_REGISTRY/net-build:$env:APPVEYOR_REPO_TAG_NAME