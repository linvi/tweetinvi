# URL Redirect Authentication

**URL redirect authentication** is better suited for Web Application.\
If you are writing a desktop application I suggest that you use the [PIN based authentication](./authentication-pin-code).

## The flow

The Redirect URL authentication process requires some additional considerations if you use multiple servers.

1. Request Twitter to provide a unique authentication URL and an associated request token.
2. Request the user to navigate to this url (or redirect the user to this url).
3. Twitter will ask the user to authenticate and authorize the app.
4. If the user accepts, Twitter will redirect the user to your website with some authentication information as query parameters.
5. Using these query parameters and the request token we can finally generate the credentials.

## Pre-requisites

To use url redirection, you will need to allow your app to redirect to your hosted website.

* Go to you app at https://developer.twitter.com/en/apps/
* In the `App Details` tab, click `Edit`
* Under `Callback URLs` add your url redirection -> `http://localhost:8042/Home/ValidateTwitterAuth`

## Authenticate with Tweetinvi

The following example is for ASP.NETCore developers using MVC. We will separate the process in 2 routes:

* `TwitterAuth` to initialize the authentication process.
* `ValidateTwitterAuth` a url that the user will be redirected to after having approved the application.

During the `TwitterAuth` authentication initialization, Twitter will send us a token. This token will need to be used after the redirection.\
We will need to store this token so that we can access these information from the `ValidateTwitterAuth`.\
The `IAuthenticationRequestStore` interface provide a simple way of storing and accessing these information.\

``` c#
private static readonly IAuthenticationRequestStore _myAuthRequestStore = new LocalAuthenticationRequestStore();

public async Task<ActionResult> TwitterAuth()
{
    var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");
    var authenticationRequestId = Guid.NewGuid().ToString();
    var redirectPath = Request.Scheme + "://" + Request.Host.Value + "/Home/ValidateTwitterAuth";

    // Add the user identifier as a query parameters that will be received by `ValidateTwitterAuth`
    var redirectURL = _myAuthRequestStore.AppendAuthenticationRequestIdToCallbackUrl(redirectPath, authenticationRequestId);
    // Initialize the authentication process
    var authenticationRequestToken = await appClient.Auth.RequestAuthenticationUrl(redirectURL);
    // Store the token information in the store
    await _myAuthRequestStore.AddAuthenticationToken(authenticationRequestId, authenticationRequestToken);

    // Redirect the user to Twitter
    return new RedirectResult(authenticationRequestToken.AuthorizationURL);
}

 public async Task<ActionResult> ValidateTwitterAuth()
{
    var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");
    
    // Extract the information from the redirection url
    var requestParameters = await RequestCredentialsParameters.FromCallbackUrl(Request.QueryString.Value, _myAuthRequestStore);
    // Request Twitter to generate the credentials.
    var userCreds = await appClient.Auth.RequestCredentials(requestParameters);

    // Congratulations the user is now authenticated!
    var userClient = new TwitterClient(userCreds);
    var user = await userClient.Users.GetAuthenticatedUser();

    ViewBag.User = user;

    return View();
}
```