$ErrorActionPreference = 'Stop';

if ( $env:APPVEYOR_PULL_REQUEST_NUMBER -Or ! $env:APPVEYOR_REPO_BRANCH.Equals("master")) {
  Write-Host Nothing to deploy.
  Exit 0
}

$files = ""
Write-Host Starting deploy
docker login -u="$env:DOCKER_USER" -p="$env:DOCKER_PASS"

if ( $env:APPVEYOR_PULL_REQUEST_NUMBER ) {
  Write-Host Pull request $env:APPVEYOR_PULL_REQUEST_NUMBER
  $files = $(git --no-pager diff --name-only FETCH_HEAD $(git merge-base FETCH_HEAD master))
} else {
  Write-Host Branch $env:APPVEYOR_REPO_BRANCH
  $files = $(git diff --name-only HEAD~1)
}

Write-Host Changed files:

$dirs = @{}

$files | ForEach-Object {
  Write-Host $_
  $dir = $_ -replace "\/[^\/]+$", ""
  $dir = $dir -replace "/", "\"
  if (Test-Path "$dir\push.ps1") {
    Write-Host "Storing $dir for deployment"
    $dirs.Set_Item($dir, 1)
  } else {
    $dir = $dir -replace "\\[^\\]+$", ""
    if (Test-Path "$dir\push.ps1") {
      Write-Host "Storing $dir for deployment"
      $dirs.Set_Item($dir, 1)
    }
  }
}

$dirs.GetEnumerator() | Sort-Object Name | ForEach-Object {
  $dir = $_.Name
  Write-Host Building in directory $dir
  Push-Location $dir
  .\push.ps1
  Pop-Location
}
