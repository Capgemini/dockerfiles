FROM microsoft/windowsservercore:10.0.14393.1480

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Visual Studio bootstrap
ENV BOOTSTRAP_DOWNLOAD_URL https://jsacapdevdata.blob.core.windows.net/artifacts/vs.exe

# .NET Core SDK
ENV DOTNET_SDK_VERSION 2.0.0
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-win-x64.zip
ENV DOTNET_SDK_DOWNLOAD_SHA C0942299437541C6B173F71213B43D2CC1E0EBC5C75F78948C0C6279A8D1C5B293999C93863392A4219F819B1DDD73D95112930B08EC8DDAA89918931E492DB0

# install build tools
RUN Invoke-WebRequest $Env:BOOTSTRAP_DOWNLOAD_URL -OutFile vs.exe -UseBasicParsing; \
    Start-Process vs.exe -ArgumentList '--add Microsoft.VisualStudio.Workload.MSBuildTools --quiet' -Wait; \
    Remove-Item -Force vs.exe

# set path to msbuild
RUN setx /M PATH $($Env:PATH + ';C:/Program Files (x86)/Microsoft Visual Studio/2017/Community/MSBuild/15.0/Bin/');

# install dotnet core sdk
RUN Invoke-WebRequest $Env:DOTNET_SDK_DOWNLOAD_URL -OutFile dotnet.zip; \
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $Env:DOTNET_SDK_DOWNLOAD_SHA) { \
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; \
        exit 1; \
    }; \
    \
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet.zip

# set path to dotnet core
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN New-Item -Type Directory warmup; \
    cd warmup; \
    dotnet new; \
    cd ..; \
    Remove-Item -Force -Recurse warmup