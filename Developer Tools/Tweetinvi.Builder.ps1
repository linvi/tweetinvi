Param(
	$v = '1.3.0.0',			     # Version number
	$m = 'Release',              # Visual Studio Build Mode
	[Switch]$dnr,				 # Do Not Rebuild 
	[Switch]$h,					 # Help
	[Switch]$help,				 # Help
	[Switch]$iel,				 # Include External Libraries	
	[Switch]$uv,				 # Update Version Only
	[Switch]$nugetMultipleDLLs,  # Add non merged DLLs to nuget folders
	[Switch]$b,					 # Build only,
	[Switch]$sign				 # Sign the output library
);

$version = $v;
$netCoreVersion = $v + '-*';
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
	Write-Host 'NUGET Arguments'
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
$nugetToolsFolder = '.\TweetinviAPI\tools'
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

# .NET Core variables
$netCoreRootPath = '..\Tweetinvi.NETCore\'
$netCoreExamplinvi = 'Tweetinvi.NETCore/Examplinvi'
$netCoreNugetFolder = '.\TweetinviAPI\lib\netstandard1.6'
$netCoreTemp = 'temp_net_core_' + $version;

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
$replaceNugetReleaseNotesRegex = '<releaseNotes>https://github.com/linvi/tweetinvi/releases/tag/[0-9\.]*</releaseNotes>';
$replaceNugetReleaseNotesWith = '<releaseNotes>https://github.com/linvi/tweetinvi/releases/tag/' + $version + '</releaseNotes>';

Get-Item 'TweetinviAPI\TweetinviAPI.nuspec' | .\Replace-Regex.ps1 -Pattern $replaceNugetVersionRegex -Replacement $replaceNugetVersionWith -overwrite
Get-Item 'TweetinviAPI\TweetinviAPI.nuspec' | .\Replace-Regex.ps1 -Pattern $replaceNugetReleaseNotesRegex -Replacement $replaceNugetReleaseNotesWith -overwrite

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

# Update .NETCore versions

if ($true) # Variables scope
{
    $netCoreExamplinviPath = $netCoreRootPath + 'Examplinvi\Examplinvi.csproj'
    Write-Host Updating $netCoreExamplinviPath

    $replaceAssemblyVersionRegex = '<VersionPrefix>[0-9\.]*</VersionPrefix>'
    $replaceAssemblyVersionWith =  '<VersionPrefix>' + $version + '</VersionPrefix>'
    Get-Item $netCoreExamplinviPath | .\Replace-Regex.ps1 -Pattern $replaceAssemblyVersionRegex -Replacement $replaceAssemblyVersionWith -overwrite
}


$filePath = $rootPath + $tweetinviWebLogic + '\TwitterClientHandler.cs';
Get-Item $filePath | .\Replace-Regex.ps1 -Pattern '"Tweetinvi/(?<versionNumber>\d+(\.\d+)*)(.x)?"' -Replacement ('"Tweetinvi/' + $version + '"') -overwrite

if (!$uv.IsPresent) {
	# Restore Nuget Packages
	Write-Host 'Restoring nuget packages';

	$p = Start-Process -Filepath '.\nuget.exe' -ArgumentList 'restore ../' -PassThru -NoNewWindow;
	$null = $p.WaitForExit(-1);
	git checkout -- ../Examplinvi.UniversalApp/project.lock.json

	# Remove previous binaries
	$examplinviBin = $rootPath + $examplinvi + '\bin\' + $releaseMode
    $netCoreExamplinviBin = $netCoreRootPath + $examplinvi + '\bin\' + $releaseMode + '\netcoreapp1.0'
	
	if (Test-Path $examplinviBin) {
		rmdir -r $examplinviBin
	}

    if (Test-Path $netCoreExamplinviBin) {
		rmdir -r $netCoreExamplinviBin
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

    If (Test-Path $netCoreTemp)
	{
		Remove-Item -r $netCoreTemp\*
	}
	Else
	{
		mkdir $netCoreTemp
	}

	# Build Portable solution
    if (!$dnr.IsPresent)
    {
	    Build $rootPath'Tweetinvi.sln' $releaseMode

        Write-Host ".NET CORE Automatic build is not yet implemented. Please ensure Examplinvi for .NET Core is compiled in " + $releaseMode
        Read-Host 'Press Enter to continue…' | Out-Null
    }

    # Build .NET CORE solution


	# Move dll into temporary folder
	Get-ChildItem -LiteralPath $examplinviBin -filter *.dll  | % { Copy-Item $_.fullname $temporaryFolder }
    Get-ChildItem -LiteralPath $netCoreExamplinviBin -filter *.dll  | % { Copy-Item $_.fullname $netCoreTemp }

    cp $env:USERPROFILE\.nuget\packages\Newtonsoft.Json\9.0.1\lib\netstandard1.0\Newtonsoft.Json.dll $netCoreTemp
    cp $env:USERPROFILE\.nuget\packages\Autofac\4.1.0\lib\netstandard1.1\Autofac.dll $netCoreTemp
    cp 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\1.0.4\System.Reflection.TypeExtensions.dll' $netCoreTemp
    

	if ($b.IsPresent) {
		Exit;
	}
	
	# Ensure the nuget folders have been created
	mkdir $net40Folder -Force | Out-Null
	mkdir $net45Folder -Force | Out-Null
	mkdir $net40PortableFolder -Force | Out-Null
	mkdir $net45PortableFolder -Force | Out-Null
    mkdir $netCoreNugetFolder -Force | Out-Null

	# Ensure the nuget folders are empty
	rm -Force ($net40Folder + '\*');
	rm -Force ($net45Folder + '\*');
	rm -Force ($net40PortableFolder + '\*');
	rm -Force ($net45PortableFolder + '\*');
    rm -Force ($netCoreNugetFolder + '\*');

    # Add Cheatsheet to help developers
    Copy-Item $rootPath$examplinvi\Program.cs $temporaryFolder\Cheatsheet.cs
	Copy-Item $rootPath$examplinvi\Program.cs $nugetToolsFolder\Cheatsheet.cs

    # Prepare folders for Merged Assemblies
    $outputFolder = $temporaryFolder + '\output';
    mkdir $outputFolder -Force | Out-Null

    $netCoreOutputFolder = $netCoreTemp + '\output';
    mkdir $netCoreOutputFolder -Force | Out-Null

	$mergedDLLPath = $temporaryFolder + '\output\' + $tweetinviAPIMerged
    $netCoreMergedDLLPath = $netCoreTemp + '\output\' + $tweetinviAPIMerged

	$ILMergeCommand = '.\ILMerge.exe /target:library /out:' + $mergedDLLPath + ' /keyfile:../tweetinvi.snk '
    $netCoreILMergeCommand = '.\ILMerge.exe /target:library /out:' + $netCoreMergedDLLPath + ' /keyfile:../tweetinvi.snk '

	$dllMergeParam = "";
    $netCoreDLLMergeParam = "";

	# Create Merged assembly

    if ($iel.IsPresent)
    {
	    for ($i=0; $i -lt $additionalAssemblies.length; $i++)
	    {
			$dllMergeParam = $dllMergeParam  + $temporaryFolder + '\' + $additionalAssemblies[$i] + ' ';
		    $ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $additionalAssemblies[$i] + ' ';
            
            $netCoreDLLMergeParam = $netCoreDLLMergeParam  + $netCoreTemp + '\' + $additionalAssemblies[$i] + ' ';
            $netCoreILMergeCommand = $netCoreILMergeCommand +  $netCoreTemp + '/' + $additionalAssemblies[$i] + ' ';
	    }
    }

	for ($i=4; $i -lt $projects.length; $i++) # start at 4 as there are 4 projects that are not part of the library core
	{
		$dllMergeParam = $dllMergeParam  + $temporaryFolder + '\' + $additionalAssemblies[$i] + ' ';
		$ILMergeCommand = $ILMergeCommand +  $temporaryFolder + '/' + $projects[$i] + '.dll ';

        $netCoreDLLMergeParam = $netCoreDLLMergeParam  + $netCoreTemp + '\' + $additionalAssemblies[$i] + ' ';
		$netCoreILMergeCommand = $netCoreILMergeCommand +  $netCoreTemp + '\' + $projects[$i] + '.dll ';
	}

	
	# Start-Process -wait -FilePath ".\ILMerge.exe" -ArgumentList "/target:library", ("/out:" + $mergedDLLPath), "/keyfile:../tweetinvi.snk", $dllMergeParam -NoNewWindow
	Write-Host $ILMergeCommand
    Invoke-Expression $ILMergeCommand | Out-Null

    Write-Host $netCoreILMergeCommand
    Invoke-Expression $netCoreILMergeCommand | Out-Null
	
	if ($sign.IsPresent) 
	{
		$signToolExe = "C:\Program Files (x86)\Windows Kits\10\bin\x86\signtool.exe"
		$certPath = "/f tweetinvi.certificate.p12";
		$certPassword = '/p "' + [Environment]::GetEnvironmentVariable("tweetinvikey", "User") + '"';
		$certTimestamp = '/t "http://timestamp.verisign.com/scripts/timstamp.dll"';

		Write-Host($signToolExe + " sign " + $certPath + " " + $certPassword + " " + $certTimestamp + " " + $mergedDLLPath)
		
		Start-Process -Wait -FilePath $signToolExe -ArgumentList " sign ",$certPath,$certPassword,$certTimestamp,$mergedDLLPath -PassThru -NoNewWindow;
		
		$certPaths = "";
		
		for ($i=4; $i -lt $projects.length; $i++) # start at 4 as there are 4 projects that are not part of the library core
		{
			$certPaths = $certPaths + $temporaryFolder + '\' + $projects[$i] + '.dll ';
		}
		
		Start-Process -Wait -FilePath $signToolExe -ArgumentList " sign ",$certPath,$certPassword,$certTimestamp,$certPaths -PassThru -NoNewWindow;
	}

	if ($nugetMultipleDLLs.IsPresent)
	{
        # Copy *.dll into nuget folders
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40Folder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45Folder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net40PortableFolder }
		Get-ChildItem -LiteralPath $examplinviBin -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45PortableFolder }
	}

	
	if (!$nugetMultipleDLLs.IsPresent) {
        # Copy Merged DLL into Nuget folder

		Write-Host 'Copying merged DLL into nuget...' 
		Write-Host $mergedDLLPath;

		$mergedDLLPath = '.\' + $mergedDLLPath;

		Copy-Item $mergedDLLPath ($net40Folder + '\Tweetinvi.dll');
		Copy-Item $mergedDLLPath ($net45Folder + '\Tweetinvi.dll');
		Copy-Item $mergedDLLPath ($net40PortableFolder + '\Tweetinvi.dll');

		Get-ChildItem -LiteralPath $temporaryFolder -filter Tweetinvi*.dll  | % { Copy-Item $_.fullname $net45PortableFolder }
        Get-ChildItem -LiteralPath $netCoreTemp -filter Tweetinvi*.dll | % { Copy-Item $_.fullname $netCoreNugetFolder }
	}

	# Create Zip files
	$tweetinviBinariesPackage = 'Tweetinvi ' + $version + ' - Binaries.zip'
	$tweetinviMergedBinariesPackage = 'Tweetinvi ' + $version + ' - Merged Binaries.zip'

	Write-Zip -OutputPath $tweetinviBinariesPackage (dir $temporaryFolder)
	Write-Zip -OutputPath $tweetinviMergedBinariesPackage (ls $temporaryFolder\$tweetinviAPIMerged,  $temporaryFolder\Cheatsheet.cs)

	#Cleanup
	rm DTAR_*

    if (Test-Path 'obj')
    {
	    rm -r obj
    }
    
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