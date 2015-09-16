Param([Switch]$uv);

$version='0.9.10.0'
$assemblyinfoLocation = 'Properties\assemblyinfo.cs'
$rootPath = '..\'
$releaseMode = 'Release' # vs. 'Debug'
$temporaryFolder = 'temp_' + $version
$net40Folder = '.\TweetinviAPI\lib\net40'
$net45Folder = '.\TweetinviAPI\lib\net45'
$net40PortableFolder = '.\TweetinviAPI\lib\portable-net40+sl5+wp80+win8+wpa81'
$net45PortableFolder = '.\TweetinviAPI\lib\portable-net45+wp80+win8+wpa81+dnxcore50'
$tweetinviAPIMerged = 'TweetinviAPI.dll'

$examplinvi = 'Examplinvi'
$testinvi = 'Testinvi'
$tweetinvi = 'Tweetinvi'
$tweetinviSecurity = 'Tweetinvi.Security'
$tweetinviControllers = 'Tweetinvi.Controllers'
$tweetinviCore = 'Tweetinvi.Core'
$tweetinviCredentials = 'Tweetinvi.Credentials'
$tweetinviFactories = 'Tweetinvi.Factories'
$tweetinviLogic = 'Tweetinvi.Logic'
$tweetinviWebLogic = 'Tweetinvi.WebLogic'
$tweetinviStreams = 'Tweetinvi.Streams'

$projects = 
@(
	# Other projects
	$examplinvi,
	$testinvi,
	
	# Tweetinvi API
	$tweetinvi,
	$tweetinviSecurity, 
	$tweetinviControllers,
	$tweetinviCore,
	$tweetinviCredentials,
	$tweetinviFactories,
	$tweetinviLogic,
	$tweetinviWebLogic,
	$tweetinviStreams
)

$additionalAssemblies = 
@(
	'Autofac.dll',
	'Newtonsoft.Json.dll',  
	'Microsoft.Threading.Tasks.dll',
	'Microsoft.Threading.Tasks.Extensions.dll',
    'System.Net.Http.Extensions.dll',
    'System.Net.Http.Primitives.dll'
)

# Loading dependencies
. .\Visual.Studio.Builder.ps1

# Update application version

$replaceNugetVersionRegex = '<version>[0-9\.]*</version>';
$replaceNugetVersionWith = '<version>' + $version + '</version>';

Get-Item 'TweetinviAPI\TweetinviAPI.nuspec' | .\Replace-Regex.ps1 -Pattern $replaceNugetVersionRegex -Replacement $replaceNugetVersionWith -overwrite

for ($i=0; $i -lt $projects.length; $i++)
{
	$filePath = $rootPath + $projects[$i] + '\' + $assemblyinfoLocation
	Write-Host Updating $filePath

	$replaceAssemblyVersionRegex = '\[assembly: AssemblyVersion\("(?<versionNumber>\d+(\.\d+)*)"\)\]'
	$replaceAssemblyVersionWith = '[assembly: AssemblyVersion("' + $version + '")]'

	Get-Item $filePath | .\Replace-Regex.ps1 -Pattern $replaceAssemblyVersionRegex -Replacement $replaceAssemblyVersionWith -overwrite

	$replaceAssemblyFileVersionRegex = '\[assembly: AssemblyFileVersion\("(?<versionNumber>\d+(\.\d+)*)"\)\]'
	$replaceAssemblyFileVersion = '[assembly: AssemblyFileVersion("' + $version + '")]'

	Get-Item $filePath | .\Replace-Regex.ps1 -Pattern $replaceAssemblyFileVersionRegex -Replacement $replaceAssemblyFileVersion -overwrite
}

$filePath = $rootPath + $tweetinviWebLogic + '\TwitterClientHandler.cs';
Get-Item $filePath | .\Replace-Regex.ps1 -Pattern '"Tweetinvi/(?<versionNumber>\d+(\.\d+)*)(.x)?"' -Replacement ('"Tweetinvi/' + $version + '"') -overwrite


if (!$uv.IsPresent) {
	# Build solution
	Build $rootPath'Tweetinvi.sln' $releaseMode

	# Create temporary folder
	If (Test-Path $temporaryFolder)
	{
		Remove-Item $temporaryFolder\*
	}
	Else
	{
		mkdir $temporaryFolder
	}

	# Move dll into temporary folder
	$examplinviBin = $rootPath + $examplinvi + '\bin\' + $releaseMode

	Get-ChildItem -LiteralPath $examplinviBin -filter *.dll  | % { Copy-Item $_.fullname $temporaryFolder }

	Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40Folder }
	Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45Folder }
	Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40PortableFolder }
	Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45PortableFolder }

	Copy-Item $rootPath$examplinvi\Program.cs $temporaryFolder\Cheatsheet.cs

	# Create Merged assembly
	$ILMergeCommand = '.\ILMerge.exe /target:library /out:' + $temporaryFolder + '/' + $tweetinviAPIMerged + ' '

	for ($i=0; $i -lt $additionalAssemblies.length; $i++)
	{
		$ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $additionalAssemblies[$i] + ' '
	}

	for ($i=2; $i -lt $projects.length; $i++)
	{
		$ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $projects[$i] + '.dll '
	}

	Write-Host $ILMergeCommand
	Invoke-Expression $ILMergeCommand

	# Create Zip files
	$tweetinviBinariesPackage = 'Tweetinvi ' + $version + ' - Binaries.zip'
	$tweetinviMergedBinariesPackage = 'Tweetinvi ' + $version + ' - Merged Binaries.zip'

	Write-Zip -OutputPath $tweetinviBinariesPackage (dir $temporaryFolder)
	Write-Zip -OutputPath $tweetinviMergedBinariesPackage (ls $temporaryFolder\$tweetinviAPIMerged,  $temporaryFolder\Cheatsheet.cs)

	#Cleanup
	rm DTAR_*
	rm -r .\obj
	$answer = Read-Host "Do you want to cleanup the temporary files? (y/n)"

	while("y", "yes", "n", "no" -notcontains $answer)
	{
		$answer = Read-Host "Yes or No"
	}

	if ($answer -eq "y" -or $answer -eq "yes")
	{
		Remove-Item $temporaryFolder -Force -Recurse
		Write-Host Temporary files successfully removed!
	}
}

Write-Host Sript successfully terminated!