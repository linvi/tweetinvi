**********************************************************
****************** CONFIGURE MACHINE *********************
**********************************************************


Install PowerShell Community Extensions to get the Write-Zip command

Add the PowerShell.Exe.Config to C:\Windows\System32\WindowsPowerShell\v1.0
Run : Set-ExecutionPolicy RemoteSigned

Restart the machine (pscx cannot be used otherwise)


**********************************************************
************************ NUGET ***************************
**********************************************************


nuget pack
nuget push <*.nupkg> -ApiKey 'MY_NUGET.ORG_APIKEY' -Verbosity detailed


**********************************************************
********************** VS SIGNING ************************
**********************************************************


Open Visual Studio Command Prompt

// Generate the public Key file
sn -p tweetinvi.pfx tweetinvi.key

// Get the Hexa version of the public key
// When performing this action the password should be requested
sn -tp tweetinvi.key 