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


**********************************************************
********************** DLL SIGNING ************************
**********************************************************

// From ILMerge with *.snk

ILMerge /target:library /out:merged.dll /keyfile:tweetinvi.snk some.dll someother.dll


// From ILMerge with *.pfx
// http://blog.nerdbank.net/2009/06/how-to-get-ilmerge-to-work-with-pfx.html

sn -p tweetinvi.pfx tweetinvi.pub
ilmerge /keyfile:tweetinvi.pub /delaysign /out:output\Tweetinvi.dll some.dll someother.dll
sn -R output\Tweetinvi.dll some.pfx


// From non signed DLL
// http://www.geekzilla.co.uk/ViewCE64BEF3-51A6-4F1C-90C9-6A76B015C9FB.htm

ildasm Tweetinvi.dll /out:Tweetinvi.il
ren Tweetinvi.dll Tweetinvi.dll.orig
ilasm Tweetinvi.il /dll /key= key_path.snk