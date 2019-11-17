Param(
    $v = '4.0.3', # Version number
    $m = 'Release', # Visual Studio Build Mode
    [Switch]$dnr, # Do Not Rebuild 
    [Switch]$h, # Help
    [Switch]$help, # Help
    [Switch]$iel, # Include External Libraries	
    [Switch]$uv, # Update Version Only
    [Switch]$nugetMultipleDLLs, # Add non merged DLLs to nuget folders
    [Switch]$b, # Build only,
    [Switch]$sign			 # Sign the output library
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

if ($iel.IsPresent -and !$nugetMultipleDLLs.IsPresent) {
    Write-Host 'Nuget should never have merged binaries containing the external dependencies.'
    Write-Host '-iel and -nugetMergedDLLs cannot be used together.'
    return;
}

$assemblyinfoLocation = 'Properties\assemblyinfo.cs'
$rootPath = '..\'
$nugetToolsFolder = '.\TweetinviAPI\tools'
$netFrameworkFolder = '.\TweetinviAPI\lib\net461'

$tweetinviAPIMerged = 'Tweetinvi.dll'

$examplinvi = 'Examplinvi.NETFramework'

$tweetinvi = 'Tweetinvi'
$tweetinviSecurity = 'Tweetinvi.Security'
$tweetinviControllers = 'Tweetinvi.Controllers'
$tweetinviCore = 'Tweetinvi.Core'
$tweetinviCredentials = 'Tweetinvi.Credentials'
$tweetinviFactories = 'Tweetinvi.Factories'
$tweetinviLogic = 'Tweetinvi.Logic'
$tweetinviWebLogic = 'Tweetinvi.WebLogic'
$tweetinviStreams = 'Tweetinvi.Streams'
$tweetinviWebhooks = 'Tweetinvi.Webhooks'

# .NET Core variables
$netCoreRootPath = '..\'
$netCoreExamplinvi = 'Examplinvi.NETStandard-2.0'
$netCoreExamplinviPath = $netCoreRootPath + $netCoreExamplinvi
$netCoreNugetFolder = '.\TweetinviAPI\lib\netstandard1.4'
$netCoreTemp = 'temp_net_core_' + $version;

$projects = 
@(
    # Tweetinvi API
    $tweetinvi,
    $tweetinviSecurity, 
    $tweetinviControllers,
    $tweetinviCore,
    $tweetinviCredentials,
    $tweetinviFactories,
    $tweetinviLogic,
    $tweetinviWebLogic,
    $tweetinviStreams,
    $tweetinviWebhooks
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

# Update .NETCore versions

for ($i = 0; $i -lt $projects.length; $i++) {
    $filePath = $rootPath + $projects[$i] + '\' + $projects[$i] + '.csproj'
    Write-Host Updating $filePath

    $replaceAssemblyVersionRegex = '<VersionPrefix>[0-9\.]*</VersionPrefix>'
    $replaceAssemblyVersionWith = '<VersionPrefix>' + $version + '</VersionPrefix>'
    Get-Item $filePath | .\Replace-Regex.ps1 -Pattern $replaceAssemblyVersionRegex -Replacement $replaceAssemblyVersionWith -overwrite
}

# Update .NETCore HTTP Request Handler
$filePath = $rootPath + $tweetinviWebLogic + '\TwitterClientHandler.cs';
Get-Item $filePath | .\Replace-Regex.ps1 -Pattern '"Tweetinvi/(?<versionNumber>\d+(\.\d+)*)(.x)?"' -Replacement ('"Tweetinvi/' + $version + '"') -overwrite

$netCoreExamplinviBin = $netCoreExamplinviPath + '\bin\' + $releaseMode + '\netcoreapp2.0'

if (!$uv.IsPresent) {

    # Restore Nuget Packages
    Write-Host 'Restoring nuget packages';

    $p = Start-Process -Filepath '.\nuget.exe' -ArgumentList 'restore ../Tweetinvi.NETCore.sln' -PassThru -NoNewWindow;
    $null = $p.WaitForExit(-1);
    git checkout -- ../Examplinvi.UniversalApp/project.lock.json

    # Remove previous binaries

    if (Test-Path $netCoreExamplinviBin) {
        rmdir -r $netCoreExamplinviBin
    }

    # Create temporary folder
    If (Test-Path $netCoreTemp) {
        Remove-Item -r $netCoreTemp\*
    }
    Else {
        mkdir $netCoreTemp
    }

    # Build Portable solution
    if (!$dnr.IsPresent) {
        BuildNetCore $netCoreExamplinviPath $releaseMode
    }

    # Move dll into temporary folder
    Get-ChildItem -LiteralPath $netCoreExamplinviBin -filter *.dll | % { Copy-Item $_.fullname $netCoreTemp }

    cp $env:USERPROFILE\.nuget\packages\Newtonsoft.Json\10.0.2\lib\netstandard1.0\Newtonsoft.Json.dll $netCoreTemp
    cp $env:USERPROFILE\.nuget\packages\Autofac\4.6.0\lib\netstandard1.1\Autofac.dll $netCoreTemp
    cp $env:USERPROFILE\.nuget\packages\nito.asyncex.context\1.1.0\lib\netstandard1.3\Nito.AsyncEx.Context.dll $netCoreTemp
    cp 'C:\Program Files\dotnet\shared\Microsoft.NETCore.App\2.0.9\System.Reflection.TypeExtensions.dll' $netCoreTemp 

    if ($b.IsPresent) {
        Exit;
    }

    # Ensure the nuget folders have been created
    mkdir $netFrameworkFolder -Force | Out-Null
    mkdir $netCoreNugetFolder -Force | Out-Null

    # Ensure the nuget folders are empty
    rm -Force ($netFrameworkFolder + '\*');
    rm -Force ($netCoreNugetFolder + '\*');

    # Add Cheatsheet to help developers
    Copy-Item $rootPath$examplinvi\Program.cs $nugetToolsFolder\Cheatsheet.cs

    # Prepare folders for Merged Assemblies
    $netCoreOutputFolder = $netCoreTemp + '\output';
    mkdir $netCoreOutputFolder -Force | Out-Null

    $netCoreMergedDLLPath = $netCoreTemp + '\output\' + $tweetinviAPIMerged
    $netCoreILMergeCommand = '.\ILMerge.exe /target:library /out:' + $netCoreMergedDLLPath + ' /keyfile:../tweetinvi.snk '

    $netCoreDLLMergeParam = "";

    # Create Merged assembly

    if ($iel.IsPresent) {
        for ($i = 0; $i -lt $additionalAssemblies.length; $i++) {
            $netCoreDLLMergeParam = $netCoreDLLMergeParam + $netCoreTemp + '\' + $additionalAssemblies[$i] + ' ';
            $netCoreILMergeCommand = $netCoreILMergeCommand + $netCoreTemp + '/' + $additionalAssemblies[$i] + ' ';
        }
    }

    for ($i = 0; $i -lt $projects.length; $i++) {
        # start at 4 as there are 4 projects that are not part of the library core
        $netCoreDLLMergeParam = $netCoreDLLMergeParam + $netCoreTemp + '\' + $additionalAssemblies[$i] + ' ';
        $netCoreILMergeCommand = $netCoreILMergeCommand + $netCoreTemp + '\' + $projects[$i] + '.dll ';
    }

    # ILMerge and Strong Name

    # Start-Process -wait -FilePath ".\ILMerge.exe" -ArgumentList "/target:library", ("/out:" + $dllPath), "/keyfile:../tweetinvi.snk", $netCoreDLLMergeParam -NoNewWindow
    Write-Host $netCoreILMergeCommand
    Invoke-Expression $netCoreILMergeCommand | Out-Null
	
    # Signing
    if (!$sign.IsPresent) {
        $signToolExe = "C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x86\signtool.exe"
        $certPath = "/f tweetinvi.certificate.private.p12";
        $certPassword = '/p "' + [Environment]::GetEnvironmentVariable("tweetinvikey", "User") + '"';
        $certTimestamp = '/t "http://timestamp.verisign.com/scripts/timstamp.dll"';
		
        $certPaths = "";
		
        for ($i = 0; $i -lt $projects.length; $i++) {
            $certPaths = $certPaths + $nugetToolsFolder + '\' + $projects[$i] + '.dll ';
        }

        # !!! THIS WILL CURRENTLY FAIL AS THE CODE SIGNING CERTIFICATE HAS EXPIRED

        # PS .\signtool.exe sign /debug /f .\tweetinvi.certificate.private.p12 /p "THE_PASSWORD" /t "http://timestamp.verisign.com/scripts/timstamp.dll" .\temp_2.1.0.0\output\Tweetinvi.dll
        Start-Process -Wait -FilePath $signToolExe -ArgumentList " sign ", $certPath, $certPassword, $certTimestamp, $certPaths -PassThru -NoNewWindow;
    }

    # Copy *.dll into nuget folders
    Get-ChildItem -LiteralPath $netCoreTemp -filter Tweetinvi*.dll | % { Copy-Item $_.fullname $netCoreNugetFolder }
    Get-ChildItem -LiteralPath $netCoreTemp -filter Tweetinvi*.dll | % { Copy-Item $_.fullname $netFrameworkFolder }

    # Create Zip files
    $tweetinviBinariesPackage = 'Tweetinvi ' + $version + ' - Binaries.zip'
    Write-Zip -OutputPath $tweetinviBinariesPackage (ls $netCoreTemp)

    #Cleanup
    rm DTAR_*

    if (Test-Path 'obj') {
        rm -r obj
    }
    
    $answer = Read-Host "Do you want to cleanup the temporary files? (y/n)"

    while ("y", "yes", "n", "no" -notcontains $answer) {
        $answer = Read-Host "Yes or No"
    }

    if ($answer -eq "y" -or $answer -eq "yes") {
        Remove-Item $netCoreTemp -Force -Recurse
        Write-Host Temporary files successfully removed!
    }
}

Write-Host Script successfully terminated!