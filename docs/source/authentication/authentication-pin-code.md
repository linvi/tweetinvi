# PIN based Authentication

**PIN based authentication** is better suited for desktop applications.\
If you are writing a web application I suggest that you use the [URL redirect authentication](./authentication-url-redirect).

## The flow

The PIN based authentication process is quite simple. 

1. Request Twitter to provide a unique URL that enables a user to authenticate and retrieve a captcha.
2. Ask the user to go to this URL.
3. Twitter will ask the user to authenticate and accept the permissions requested by your Twitter application.
4. If the user accepts, Twitter generates a PIN Code and gives it to the user.
5. With this code, Twitter can now issue a new OAuth Token available from a WebRequest.

Now let's see how Tweetinvi simplifies this process.

## Authenticate with Tweetinvi

``` c#
// Create a client for your app
var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");

// Start the authentication process
var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();

// Go to the URL so that Twitter authenticates the user and gives him a PIN code.
Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
{
    UseShellExecute = true
});

// Ask the user to enter the pin code given by Twitter
Console.WriteLine("Please enter the code and press enter.");
var pinCode = Console.ReadLine();

// With this pin code it is now possible to get the credentials back from Twitter
var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);

// You can now save those credentials or use them as followed
var userClient = new TwitterClient(userCredentials);
var user = await userClient.Users.GetAuthenticatedUserAsync();

Console.WriteLine("Congratulation you have authenticated the user: " + user);
```