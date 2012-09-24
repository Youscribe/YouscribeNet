
$path = (Split-Path -Parent $MyInvocation.MyCommand.path)

include (Join-Path $path "settings.ps1")
include (Join-Path $path "assemblyinfo.ps1")
include (Join-Path $path "msbuild.ps1")
include (Join-Path $path "nuget.ps1")
include (Join-Path $path "xunit.ps1")

Task Default -depends Nuget-Bootstrap, Compile, Run-Tests, Nuget-Push

Task Compile -depends Version-AssemblyInfo, Invoke-MSBuild