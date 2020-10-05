# Account Activity

> Account Activities let you track actions performed by a user in live.\
> For example Account Activity will trigger an event when a user publishes a tweet, reads a message, likes a tweet...

## Introduction

Account Activity is a large topic that we will cover in multiple pages.\
Account Activity is a way to receive live events about actions performed by a Twitter user.

To receive such events, Twitter uses [Webhooks](#what-is-a-webhook). Behind a webhook url is a server processing requests.\
Tweetinvi offers [request handlers](#webhooks-requests-handler) which will automatically filter and respond to requests coming from Twitter.\
There are [2 ways of using handlers](#webhooks-requests-handler), through a [middleware for AspNet](./account-activity-with-aspnet) or by [manually feeding the handlers with a Http server request](./account-activity-with-http-server).

Once the webhook application is running with the request handler, you need to [register your webhook url](#webhooks-register) to Twitter so that Twitter knows which url it should send the events to.

Twitter now know where to send events but does not know yet which which users the application should monitor.\
We need to [subscribe users to the webhook](./account-activity-subscriptions).

Now that users are subscribed to our webhook, Twitter will send events (through http requests) related to the subscribed users to our server.\
These events will be raised by the [Account Activity Stream](./account-activity-events).\
We will use this stream and watch for specific events to take actions.

## What is account activity

> Account Activities let you track actions performed by a user in live.\
> For example Account Activity will trigger an event when a user publishes a tweet, reads a message, likes a tweet...

It is Twitter replacement of the old User Stream.\
Differently from the user stream, Account Activity uses Webhooks.

You can learn more as to what Account Activities offer on [twitter documentation](https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/overview).

<div class="warning">

Account Activities api is restricted to premium accounts.

* You need to request access to a premium Twitter account. You can register to get such an account on [https://developer.twitter.com/en/apply](https://developer.twitter.com/en/apply)
* After you have been approved you will be able to go to [Dev Environments](https://developer.twitter.com/en/account/environments).
* There you will be able to create a free `Account Activity API Sandbox` environment.
</div>

## What is a webhook

Similar to polling, webhooks provide your application a way of consuming new event data from an endpoint.\
However, instead of sending repeated requests for new events, you provide the endpoint with a URL, which your application monitors.\
Whenever a new event occurs within the endpoint app, it posts the event data to your specified URL, updating your application in real-time.

In Twitter, you register your application url to a webhook environment.\
When your application url has been registered, Twitter will send user events to this url.

## AspNetPlugin

`TweetinviAPI.AspNetPlugin` package offers additional tools to work with webhooks.

* It can split requests coming from Twitter and requests coming from actual users.
* It can automatically route `HttpRequest` to let Tweetinvi respond on its own.
* It provides an AspNet middleware to automatically route the `HttpRequest`.

Install the `AspNetPlugin`.

``` sh
dotnet add package TweetinviAPI.AspNetPlugin
```

If your project targets .NETCore version >= 3.0, your project will need to have a `FrameworkReference` into your dependencies `<ItemGroup>`, next to `PackageReference`.

``` xml
<ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="TweetinviAPI" Version="5.0.1" />
    <PackageReference Include="TweetinviAPI.AspNetPlugin" Version="5.0.1" />
</ItemGroup>
```

<details>
<summary>Example.csproj</summary>

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RootNamespace>tweetinvi_hello_world</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
    <PackageReference Include="TweetinviAPI" Version="5.0.1" />
    <PackageReference Include="TweetinviAPI.AspNetPlugin" Version="5.0.1" />
  </ItemGroup>

</Project>
```
</details>

<div class="note">

In the future will be done automatically when properly supported by nuget ([issue](https://github.com/NuGet/Home/issues/9592)).
</div>

Finally in your code you will need to register the plugin. This needs to be done before a client you use for webhooks is created.

``` c#
Plugins.Add<AspNetPlugin>();

var client = new TwitterClient(creds);
```

## Ngrok

Your webhook application will need to publicly accessible through a https url.\
When developing, making your localhost publicly accessible can be complicated. Ngrok comes to help you.

Different solutions offer this, but I have been using [Ngrok](https://ngrok.com/) and I liked its simplicity.\
To use it, you will need to create an account, download the binary and execute the command `ngrok http 8042`.\
Ngrok will generate a unique https url which will be redirected to the `http://localhost:8042`.

This operation will give you a list of public url endpoints that you can use to register a webhook on Twitter.\
When [registering your application url](#webhooks-register) you will need to use the `https` endpoint.

## Webhooks - Requests handler

To reduce the complexity of Account Activity, Tweetinvi offers ways to automatically handle requests coming to your http/aspnet server.

Pick the approach the most appropriate to your needs:

|                                                    | Project type                | Description                                                                                      |
|----------------------------------------------------|-----------------------------|--------------------------------------------------------------------------------------------------|
| [Http Server](./account-activity-with-http-server) | Console/Desktop Application | Help you handle http requests and automatically handle requests coming from Twitter              |
| [AspNet Server](./account-activity-with-aspnet)    | AspNet Application          | Configure AspNet to route incoming requests and automtically handle requests coming from Twitter |

## Webhooks - Register

Twitter offer different `environments` to run webhooks.\
An environment can be linked with 1 or multiple webhooks.

During the registration process of a webhook url, Twitter will send an http request to your webhook url.\
This request is called a CRC request. Twitter expect your application to send a response containing a special authentication token.

If you use the [Tweetinvi request handler](#webhooks-requests-handler), the response to the CRC request will be done automatically for you.

``` c#
// request to register your url - a crc challenge will be received by your http server
await userClient.AccountActivity.CreateAccountActivityWebhookAsync("sandbox", "https://my-webook-url.ngrok.io");
```

This request will only be successful if Twitter receives the expected response from your server.

## Webhooks - List

You can list your environments and their associated webhooks.

``` c#
// This requires applications credentials with a bearer token
var environments = await appClient.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();

// the sandbox environment is available within the free tier
var sandboxEnvironment = environments.Single(x => x.Name == "sandbox");

// registered webhooks url are available in the Webhooks collection
var registeredWebhooks = sandboxEnvironment.Webhooks;
```

You can also request webhooks of a specific environment.

``` c#
var webhooks = await appClient.AccountActivity.GetAccountActivityEnvironmentWebhooksAsync("sandbox");
```

## Webhooks - Delete

Twitter restricts the number of webhooks url to which it will send the events.\
In the case of the sandbox environment this is limited 1 unique url.

To remove a webhook from Twitter:

``` c#
await appClient.AccountActivity.DeleteAccountActivityWebhookAsync("WEBHOOK_ENVIRONMENT", "WEBHOOK_ID");
```

``` c#
// quick solution to clean your sandbox environment
var webhooks = await appClient.AccountActivity.GetAccountActivityEnvironmentWebhooksAsync("sandbox");
if (webhooks.Length > 0)
{
    await appClient.AccountActivity.DeleteAccountActivityWebhookAsync("sandbox", webhooks[0]);
}
```
