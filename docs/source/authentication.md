# Authentication

## Overview 

Twitter allows developer's application to authenticate any Twitter user. The API gives access to two different mechanisms that we will name URL redirect authentication and PIN-based authentication.

* [PIN-Based authentication](#pin-based-authentication) is better suited for Desktop application 
* [URL redirect authentication](#url-redirect-authentication) is better suited for Web Application.

**IMPORTANT : If you are using authentication on a website please read the [Web Application Considerations](https://github.com/linvi/tweetinvi/wiki/Authentication#web-application-considerations)!**

## PIN-Based Authentication
The PIN-based authentication process is quite simple. 

1. Request Twitter to provide a unique URL that enables a user to authenticate and retrieve a captcha.
2. Ask the user to go to this URL.
3. Twitter will ask the user to authenticate and accept the permissions requested by your Twitter application.
4. If the user accepts, Twitter generates a PIN Code and gives it to the user.
5. With this code, Twitter can now issue a new OAuth Token available from a WebRequest.

Now let's see how Tweetinvi simplifies this process.

``` c#
// using Tweetinvi;

// Create a client for your app
var authenticationClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");

// Start the authentication process
var authenticationRequest = await authenticationClient.Auth.RequestAuthenticationUrl();

// Go to the URL so that Twitter authenticates the user and gives him a PIN code.
Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
{
    UseShellExecute = true
});

// Ask the user to enter the pin code given by Twitter
Console.WriteLine("Please enter the code and press enter.");
var pinCode = System.Console.ReadLine();

// With this pin code it is now possible to get the credentials back from Twitter
var userCredentials = await authenticationClient.Auth.RequestCredentialsFromVerifierCode(pinCode, authenticationRequest);

// You can now save those credentials or use them as followed
var userClient = new TwitterClient(userCredentials);
var user = await userClient.Users.GetAuthenticatedUser();

Console.WriteLine("Congratulation you have authenticated the user: " + user);
```