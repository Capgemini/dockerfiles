FROM microsoft/windowsservercore:10.0.14393.1480

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# ms build
ENV BOOTSTRAP_DOWNLOAD_URL https://jsacapdevdata.blob.core.windows.net/artifacts/vs.exe

# ms build -> install build tools
RUN Invoke-WebRequest $Env:BOOTSTRAP_DOWNLOAD_URL -OutFile vs.exe -UseBasicParsing; \
    Start-Process vs.exe -ArgumentList '--add Microsoft.VisualStudio.Workload.MSBuildTools --quiet' -Wait; \
    Remove-Item -Force vs.exe

# ms build -> set path to msbuild
RUN setx /M PATH $($Env:PATH + ';C:/Program Files (x86)/Microsoft Visual Studio/2017/BuildTools/MSBuild/15.0/Bin/');

# dotnet core SDK
ENV DOTNET_SDK_VERSION 2.0.0
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-win-x64.zip
ENV DOTNET_SDK_DOWNLOAD_SHA C0942299437541C6B173F71213B43D2CC1E0EBC5C75F78948C0C6279A8D1C5B293999C93863392A4219F819B1DDD73D95112930B08EC8DDAA89918931E492DB0

# dotnet core SDK -> install
RUN Invoke-WebRequest $Env:DOTNET_SDK_DOWNLOAD_URL -OutFile dotnet.zip; \
    if ((Get-FileHash dotnet.zip -Algorithm sha512).Hash -ne $Env:DOTNET_SDK_DOWNLOAD_SHA) { \
        Write-Host 'CHECKSUM VERIFICATION FAILED!'; \
        exit 1; \
    }; \
    \
    Expand-Archive dotnet.zip -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet.zip

# dotnet core SDK -> set path
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

# dotnet core SDK -> Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN New-Item -Type Directory warmup; \
    cd warmup; \
    dotnet new; \
    cd ..; \
    Remove-Item -Force -Recurse warmup

# net framework
ENV NET_DP_47 https://download.microsoft.com/download/A/1/D/A1D07600-6915-4CB8-A931-9A980EF47BB7/NDP47-DevPack-KB3186612-ENU.exe
ENV NET_DP_462 https://download.microsoft.com/download/E/F/D/EFD52638-B804-4865-BB57-47F4B9C80269/NDP462-DevPack-KB3151934-ENU.exe
ENV NET_DP_461 https://download.microsoft.com/download/F/1/D/F1DEB8DB-D277-4EF9-9F48-3A65D4D8F965/NDP461-DevPack-KB3105179-ENU.exe
ENV NET_DP_46 https://jsacapdevdata.blob.core.windows.net/artifacts/NDP46-TargetingPack-KB3045566.exe
ENV NET_DP_452 https://download.microsoft.com/download/4/3/B/43B61315-B2CE-4F5B-9E32-34CCA07B2F0E/NDP452-KB2901951-x86-x64-DevPack.exe

# net framework -> 47
RUN Invoke-WebRequest $Env:NET_DP_47 -OutFile framework47.exe -UseBasicParsing; \
    Start-Process framework47.exe -ArgumentList '/q' -Wait; \
    Remove-Item -Force framework47.exe

# net framework -> 462
RUN Invoke-WebRequest $Env:NET_DP_462 -OutFile framework462.exe -UseBasicParsing; \
    Start-Process framework462.exe -ArgumentList '/q' -Wait; \
    Remove-Item -Force framework462.exe

# net framework -> 461
RUN Invoke-WebRequest $Env:NET_DP_461 -OutFile framework461.exe -UseBasicParsing; \
    Start-Process framework461.exe -ArgumentList '/q' -Wait; \
    Remove-Item -Force framework461.exe

# net framework -> 46
RUN Invoke-WebRequest $Env:NET_DP_46 -OutFile framework46.exe -UseBasicParsing; \
    Start-Process framework46.exe -ArgumentList '/q' -Wait; \
    Remove-Item -Force framework46.exe

# net framework -> 452
RUN Invoke-WebRequest $Env:NET_DP_452 -OutFile framework452.exe -UseBasicParsing; \
    Start-Process framework452.exe -ArgumentList '/q' -Wait; \
    Remove-Item -Force framework452.exe

# nu get
ENV NUGET_DOWNLOAD_URL https://dist.nuget.org/win-x86-commandline/latest/nuget.exe

# nu get -> install
RUN Invoke-WebRequest $Env:NUGET_DOWNLOAD_URL -OutFile nuget.exe -UseBasicParsing; \
    Copy-Item nuget.exe -Destination (New-Item $Env:ProgramFiles\nuget -Type container -Force) -Force; \
    Remove-Item -Force nuget.exe

# nu get -> set path
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\nuget')