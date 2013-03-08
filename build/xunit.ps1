Task Init-Tests {
	New-Item $xunit.output -Type directory -Force | Out-Null
}

Task Run-Tests -Depends Init-Tests {
	Get-ChildItem $source.dir -Recurse -Include *.tests,*.integrationTests |
    ?{ ($_ -is [System.IO.DirectoryInfo]) -and !($_.FullName -match ".hg") -and (Test-Path "$($_.FullName)\bin\$($build.configuration)\$($_.Name).dll") } |
    %{
			$base = $_.FullName
			$name = $_.Name
			
			Run-Xunit (Join-Path $base (Join-Path "bin" (Join-Path $build.configuration "$name.dll"))) (Join-Path $xunit.output "$name.xml")
		}
}

function Run-Xunit {
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)]$file,
		[Parameter(Position=1,Mandatory=1)]$output
	)
	
	Write-Output "Launching xunit for $file"
	exec { & $xunit.bin "$file" /nunit "$output" }
}