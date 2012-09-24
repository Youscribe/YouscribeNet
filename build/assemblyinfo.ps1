# 
# Copyright (c) 2011, Toji Project Contributors
# 
# Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
# See the file LICENSE.txt for details.
# 

properties {
  Write-Output "Loading assembly info properties"
  
  $assemblyinfo = @{}
  $assemblyinfo.file = "$($source.dir)\GlobalAssemblyInfo.cs"
  $assemblyinfo.contents = @"
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyCompany(`"YouScribe`")]
[assembly: AssemblyProduct(`"YouScribe`")]
[assembly: AssemblyCopyright(`"Copyright © YouScribe 2012`")]
[assembly: AssemblyTrademark(`"`")]
[assembly: AssemblyCulture(`"`")]
[assembly: AssemblyVersion(`"$($build.version)`")]
[assembly: AssemblyFileVersion(`"$($build.version)`")]
"@
}

Task Version-AssemblyInfo {
	Set-Content -Value $assemblyinfo.contents -Path $assemblyinfo.file
}