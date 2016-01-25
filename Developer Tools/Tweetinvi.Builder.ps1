Param(
	$v = '0.9.10.2',			 # Version number
	$m = 'Release',              # Visual Studio Build Mode
	[Switch]$dnr,				 # Do Not Rebuild 
	[Switch]$h,					 # Help
	[Switch]$help,				 # Help
	[Switch]$iel,				 # Include External Libraries	
	[Switch]$uv,				 # Update Version Only
	[Switch]$nugetMultipleDLLs   # Add non merged DLLs to nuget folders
);

$version = $v;
$releaseMode = $m;

if ($h.IsPresent -or $help.IsPresent) {
	Write-Host 'Welcome to the builder help.'
	Write-Host 'Arguments:'
	Write-Host '-dnr  : Do Not Rebuild. Only available if the temp_x folder already contains the .dll you want to use.'
	Write-Host '-h    : Help.'
	Write-Host '-help : Help.'
	Write-Host '-iel  : Include External Libraries. Whether the external libraries should be included in the Merged Binary.'
	Write-Host '-m    : Visual Studio build mode (Release OR Debug).'
	Write-Host '-uv   : Only update assemblies'' versions.'
	Write-Host '-v    : Set the version of the assemblies and build (default:' $v').';
	Write-Host ''
	Write-Host 'NUGET Argumements'
	Write-Host '-nugetMultipleDLLs : Use multiple binaries instead of a merged one.'
	Write-Host
	return;
}

if ($iel.IsPresent -and !$nugetMultipleDLLs.IsPresent)
{
	Write-Host 'Nuget should never have merged binaries containing the external dependencies.'
	Write-Host '-iel and -nugetMergedDLLs cannot be used together.'
	return;
}

$assemblyinfoLocation = 'Properties\assemblyinfo.cs'
$rootPath = '..\'
$temporaryFolder = 'temp_' + $version
$net40Folder = '.\TweetinviAPI\lib\net40'
$net45Folder = '.\TweetinviAPI\lib\net45'
$net40PortableFolder = '.\TweetinviAPI\lib\portable-net40+sl5+wp80+win8+wpa81'
$net45PortableFolder = '.\TweetinviAPI\lib\portable-net45+wp80+win8+wpa81+dnxcore50'
$tweetinviAPIMerged = 'Tweetinvi.dll'

$examplinvi = 'Examplinvi'
$examplinviUniversalApp = 'Examplinvi.UniversalApp'
$examplinviWeb = 'Examplinvi.Web'

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
	$examplinviUniversalApp,
	$examplinviWeb,
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
	# Restore Nuget Packages
	Write-Host 'Restoring nuget packages';

	$p = Start-Process -Filepath '.\nuget.exe' -ArgumentList 'restore ../' -PassThru -NoNewWindow;
	$null = $p.WaitForExit(-1);

	# Build solution

    if (!$dnr.IsPresent)
    {
	    Build $rootPath'Tweetinvi.sln' $releaseMode
    }

	# Create temporary folder
	If (Test-Path $temporaryFolder)
	{
		Remove-Item -r $temporaryFolder\*
	}
	Else
	{
		mkdir $temporaryFolder
	}

	# Move dll into temporary folder
	$examplinviBin = $rootPath + $examplinvi + '\bin\' + $releaseMode

	Get-ChildItem -LiteralPath $examplinviBin -filter *.dll  | % { Copy-Item $_.fullname $temporaryFolder }

	# Ensure the nuget folders have been created
	mkdir $net40Folder -Force | Out-Null
	mkdir $net45Folder -Force | Out-Null
	mkdir $net40PortableFolder -Force | Out-Null
	mkdir $net45PortableFolder -Force | Out-Null

	# Ensure the nuget folders are empty
	rm -Force ($net40Folder + '\*');
	rm -Force ($net45Folder + '\*');
	rm -Force ($net40PortableFolder + '\*');
	rm -Force ($net45PortableFolder + '\*');

	# Add .dll into nuget folders

	if ($nugetMultipleDLLs.IsPresent)
	{
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40Folder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45Folder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40PortableFolder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45PortableFolder }
	}

	Copy-Item $rootPath$examplinvi\Program.cs $temporaryFolder\Cheatsheet.cs

	# Create Merged assembly

    $outputFolder = $temporaryFolder + '\output';
    mkdir $outputFolder -Force | Out-Null

	$mergedDLLPath = $temporaryFolder + '\output\' + $tweetinviAPIMerged
	$ILMergeCommand = '.\ILMerge.exe /target:library /out:' + $mergedDLLPath + ' '

    if ($iel.IsPresent)
    {

	    for ($i=0; $i -lt $additionalAssemblies.length; $i++)
	    {
		    $ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $additionalAssemblies[$i] + ' '
	    }
    }

	for ($i=4; $i -lt $projects.length; $i++) # start at 4 as there are 4 projects that are not part of the library core
	{
		$ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $projects[$i] + '.dll '
	}

	Write-Host $ILMergeCommand
	Invoke-Expression $ILMergeCommand

	# Move Merged DLL into Nuget folder
	if (!$nugetMultipleDLLs.IsPresent) {
		Write-Host 'Copying merged DLL into nuget...' 
		Write-Host $mergedDLLPath;

		$mergedDLLPath = '.\' + $mergedDLLPath;

		Copy-Item $mergedDLLPath ($net40Folder + '\Tweetinvi.dll');
		Copy-Item $mergedDLLPath ($net45Folder + '\Tweetinvi.dll');
		Copy-Item $mergedDLLPath ($net40PortableFolder + '\Tweetinvi.dll');
		Copy-Item $mergedDLLPath ($net45PortableFolder + '\Tweetinvi.dll');
	}

	# Create Zip files
	$tweetinviBinariesPackage = 'Tweetinvi ' + $version + ' - Binaries.zip'
	$tweetinviMergedBinariesPackage = 'Tweetinvi ' + $version + ' - Merged Binaries.zip'

	Write-Zip -OutputPath $tweetinviBinariesPackage (dir $temporaryFolder)
	Write-Zip -OutputPath $tweetinviMergedBinariesPackage (ls $temporaryFolder\$tweetinviAPIMerged,  $temporaryFolder\Cheatsheet.cs)

	#Cleanup
	rm DTAR_*
	rm -r obj
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

Write-Host Script successfully terminated!