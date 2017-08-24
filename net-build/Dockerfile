FROM microsoft/windowsservercore:latest

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ENV BOOTSTRAP_DOWNLOAD_URL https://jsacapdevdata.blob.core.windows.net/artifacts/vs.exe

RUN Invoke-WebRequest $Env:BOOTSTRAP_DOWNLOAD_URL -OutFile vs.exe -UseBasicParsing; \
    Start-Process vs.exe -ArgumentList '--add Microsoft.VisualStudio.Workload.MSBuildTools --quiet' -Wait; \
    Remove-Item -Force vs.exe
