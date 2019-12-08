# Tweetinvi 5.0

## Breaking Changes

* Authentication flow (previously `AuthFlow`) has been completed reviewed to properly work with the concept of TwitterClient.

## Authentication

### What has changed?

**`Auth.InitializeApplicationOnlyCredentials` moved to `TwitterClient.Auth.InitializeClientBearerToken`**

``` c#
// Tweetinvi 4.0
var appCreds = new TwitterCredentials("consumer_key", "consumer_secret");
var creds = Auth.InitializeApplicationOnlyCredentials(appCreds);

// Tweetinvi 5.0
var appClient = new TwitterClient(new TwitterCredentials("consumer_key", "consumer_secret"));
await client.Auth.InitializeClientBearerToken();
```

### New

**`CreateBearerToken`**

You can now request to request the creation of a bearer token for using your app to perform some requests.

``` c#
var bearerToken = await client.Auth.CreateBearerToken();`
```

**ReadOnly Credentials**

We added the concept of readonly credentials to ensure better control of concurrency.

When creating a `new TwitterClient(credentials)`, the provided credentials will be cloned and changed in the `credentials` object will not have any effect to the generated client.

As a result, changing a `TwitterClient` credentials will only be possible by creating a new `TwitterClient`.

## Authentication Flow

`IAuthenticationRequestStore` is a new interface that guides developers through the authentication flow of redirection urls. A default in-memory solution is provided but developers are free to implement their own.
  * `AppendAuthenticationRequestIdToCallbackUrl` append an authenticationRequestId to a url
  * `ExtractAuthenticationRequestIdFromCallbackUrl` logic to extract an authenticationRequestId from a callback url
  * `GetAuthenticationRequestFromId` returns the AuthenticationRequest from its identifier
  * `AddAuthenticationToken` stores the AuthenticationRequest information
  * `RemoveAuthenticationToken` removes an AuthenticationRequest from the store

A default implementation `new AuthenticationTokenProvider` is an implementation that allow you to make the `AuthenticationToken` accessible from an in-memory store.

### What has changed?

**`StartUrlAuthProcessParameters` changed to:**
  * `RequestPinAuthUrlParameters` for pin based authentication
    * `RequestId` is null
    * `CallbackUrl` set to **oob** as per Twitter documentation

  * `RequestUrlAuthUrlParameters` for callback based authentication
    * `RequestId` is an identifier that will be used to associate your request with the returned `AuthenticationToken.Id`
      * If no `AuthenticationTokenProvider` is defined, the `RequestId` will be a new Guid.
      * If the `AuthenticationTokenProvider` is passed in constructor, the `RequestId` will use the provider `.GenerateAuthTokenId`.
      * You can set RequestId as null but you will have to implement your own solution for identifying the request (e.g. change callbackUrl to include the identifier)
    * `CallbackUrl` has to be specified by the user


**`AuthFlow.InitAuthentication`** was moved to `TwitterClient.Auth.RequestAuthenticationUrl`

`RequestAuthenticationUrl` returns an `AuthenticationRequest` which contains the url to redirected the user and authorization information to proceed with the request of credentials.

``` c#
// Tweetinvi 4.0
Auth.SetCredentials(new TwitterCredentials("consumer_key", "consumer_secret"));
var authenticationRequest = AuthFlow.InitAuthentication(applicationCredentials);

// Tweetinvi 5.0
var appClient = new TwitterClient(new TwitterCredentials("consumer_key", "consumer_secret"));
var authenticationRequest = await authenticationClient.Auth.RequestAuthenticationUrl();
```

**`AuthFlow.CreateCredentialsFromVerifierCode` moved to `TwitterClient.Auth.RequestCredentials`**

``` c#
// Tweetinvi 4.0
Auth.SetCredentials(new TwitterCredentials("consumer_key", "consumer_secret"));
var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, authenticationContext);

// Tweetinvi 5.0
var appClient = new TwitterClient(new TwitterCredentials("consumer_key", "consumer_secret"));
var userCredentials = await authenticationClient.Auth.RequestCredentials(pinCode, authenticationContext);
```

**`AuthFlow.CreateCredentialsFromCallbackURL` moved to `TwitterClient.Auth.RequestCredentialsFromCallbackUrl`**

``` c#
// Tweetinvi 4.0
Auth.SetCredentials(new TwitterCredentials("consumer_key", "consumer_secret"));
var newCredentials = AuthFlow.CreateCredentialsFromCallbackURL(callbackURL, authenticationContext);

// Tweetinvi 5.0
var appClient = new TwitterClient(new TwitterCredentials("consumer_key", "consumer_secret"));
var userCredentials = await authenticationClient.Auth.RequestCredentialsFromCallbackUrl(callbackURL, authenticationContext);
```

### Examples

We have updated the examples for you to have an example to build from.