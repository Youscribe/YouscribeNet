properties {
	$nugetKey = "4e0322be-4988-4200-83a8-6f3227d12eae"
	
	$script = @{}
	$script.dir = (Split-Path -Parent $MyInvocation.ScriptName)
	
	$base = @{}
	$base.dir = (Split-Path -Parent $script.dir)
	$base.output = (Join-Path $base.dir "Dist")
	
	$source = @{}
	$source.dir = $base.dir
	$source.solution = @(Get-ChildItem $source.dir -Filter *.sln)[0].Name # "xxx.sln"

	$build = @{}
	$build.version = "1.0"
	if ($env:BUILD_NUMBER) { $build.version = "{0}.{1}-beta" -f $build.version, $env:BUILD_NUMBER }
	$build.configuration = "Release"
	
	$nuget = @{}
	$nuget.dir = (Join-Path $base.dir ".nuget")
	$nuget.bin = (Join-Path $nuget.dir "nuget.exe")
	$nuget.nuspec_pack = @("")
	$nuget.pushsource = "http://artefacts.societe-publica.net/"
	$nuget.sources = @("http://go.microsoft.com/fwlink/?LinkID=206669")
	$nuget.source = @($nuget.sources | ?{ $_ -ne "" -and $_ -ne $null }) -join ";"
	#$nuget.key = "4e0322be-4988-4200-83a8-6f3227d12eae"
	$nuget.output = $base.output
	$nuget.packages = (Join-Path $source.dir "packages")
	
	$xunit = @{}
	$xunit.dir = (Join-Path $base.dir (Join-Path "Librairies" "xunit-1.8"))
	$xunit.bin = (Join-Path $xunit.dir "xunit.console.clr4.x86.exe")
	$xunit.output = (Join-Path $base.dir "TestOut")
}

Write-Output "beta ?" $nugetKey 