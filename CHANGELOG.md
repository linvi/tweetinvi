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

`IAuthenticationTokenProvider` is a new interface that let developers customize how Tweetinvi retrieves the AuthenticationToken.
  * `CallbackTokenIdParameterName` query parameter added to the callback url.
  * `ExtractTokenIdFromCallbackUrl` method extracting a token from a callback url.
  * `GenerateAuthTokenId` generates an identifier that will be used by `GetAuthenticationTokenFromId`
  * `GetAuthenticationTokenFromId` retrieves an AuthenticationToken from its unique identifier
  * `AddAuthenticationToken` defines how to add a token into your store
  * `RemoveAuthenticationToken` defines how to remove a token from your store.

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

``` c#
// Tweetinvi 4.0
Auth.SetCredentials(new TwitterCredentials("consumer_key", "consumer_secret"));
var authenticationContext = AuthFlow.InitAuthentication(applicationCredentials);

// Tweetinvi 5.0
var appClient = new TwitterClient(new TwitterCredentials("consumer_key", "consumer_secret"));
var authenticationContext = await authenticationClient.Auth.RequestAuthenticationUrl();
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