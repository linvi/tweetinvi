# Credentials

Twitter offers 2 types of credentials and twitter documentation specifies the type of credentials to use for each different endpoint.

* [**Application Credentials**](#application-credentials) that let you perform operations from an application perspective.
* [**User Credentials**](#user-credentials) that let you perform operations from a user perspective.

## Application Credentials 

Twitter applications can perform requests on their own.\
Application credentials (also called `ConsumerOnlyCredentials`) can be used for authenticating users, accessing the webhooks endpoints, running some streams and accessing other endpoints.

Application credentials consist of :

* `Required` CONSUMER_KEY
* `Required` CONSUMER_SECRET
* `Optional` BEARER_TOKEN (learn more [here](https://developer.twitter.com/en/docs/basics/authentication/oauth-2-0))

``` c#
// create a consumer only credentials
var appCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET")
{
    BearerToken = "BEARER_TOKEN" // bearer token is optional in some cases
};

var appClient = new TwitterClient(appCredentials);
```

### Get a Bearer Token

Application credentials can be used with or without a `Bearer Token` depending on the operation.\
Usually to run requests on twitter with application credentials you will need to specify the `Bearer Token`.

You can generate a bearer token as followed.

``` c#
// get the bearer token and create a client
var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET");
var appClientWithoutBearer = new TwitterClient(consumerOnlyCredentials);

var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();
var appCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET") 
{
    BearerToken = bearerToken
};

var appClient = new TwitterClient(appCredentials);
```

``` c#
// or you simply initialize the bearer token of client
var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET");
var appClient = new TwitterClient(consumerOnlyCredentials);

await appClient.Auth.InitializeClientBearerTokenAsync();

// the appClient requests will now use the bearer token
```

## User Credentials

User credentials is a combination of user keys and consumer keys.\
They a user to authenticate through a specific Twitter application.

To perform an operation on behalf of a user (e.g. publishing a tweet), an application will need to execute this operation with the user credentials.

User credentials consist of :

* `Required` CONSUMER_KEY
* `Required` CONSUMER_SECRET
* `Required` ACCESS_TOKEN
* `Required` ACCESS_TOKEN_SECRET

``` c#
var userCredentials = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
var userClient = new TwitterClient(appCredentials);
```

To request user credentials, you need to ask this user to authenticate and authorise your application.\
[Learn more about authentication](../authentication/authentication)