[CmdletBinding()]
Param(
	[string]$CoverageFile
)

$open_cover = (Get-ChildItem -File -Name Opencover.Console.exe -Path .\tools\ -Recurse | % { Resolve-Path (Join-Path .\tools\ $_) })[0]
& $open_cover -target:"dotnet.exe" -targetargs:"test .\src\Vertica.Utilities.Tests\Vertica.Utilities.Tests.csproj -c Coverage -f net45 -v q" -filter:"+[Vertica.Utilities]*" -mergebyhash -skipautoprops -register:user -output:$CoverageFile -hideskipped:"File;Filter;MissingPdb" -threshold:10000