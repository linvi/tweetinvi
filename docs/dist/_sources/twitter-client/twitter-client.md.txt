# Twitter Client

The Twitter client is an object centralizing all operations that can be performed via Tweetinvi.

A Twitter client is associated with a specific set of credentials which you provide when creating the client.\
All operations performed by the client will use the associated credentials.

A Twitter client can be used and act as an application or as a user.

``` c#
// Application client
var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");

// User client
var userClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
```

## Config

The Twitter client uses a default configuration that can be changed to match your needs.

### Proxy

Configure a proxy for your application to connect to the internet.

``` c#
client.Config.ProxyConfig = new ProxyConfig("my-proxy-url")
{
    Credentials = new NetworkCredential("username", "password")
};
```

### HttpRequest Timeout

You can specify the maximum duration of some operations.

``` c#
client.Config.HttpRequestTimeout = TimeSpan.FromSeconds(20);
```

### TweetMode

Older aplications need to use the old format of tweet objects. You can do so by specifying the `TweetMode` at a client level or per request.

``` c#
// will add tweet_mode=compat for any tweet request 
client.Config.TweetMode = TweetMode.Compat;

// https://api.twitter.com/1.1/statuses/update.json?status=hello&tweet_mode=compat
await client.Tweets.PublishTweetAsync("hello");
```

Configuring the `TweetMode` of a request parameters will take priority over the `TweetMode` value set in the client config.

``` c#
client.Config.TweetMode = TweetMode.Compat;

// https://api.twitter.com/1.1/statuses/update.json?status=hello&tweet_mode=extended 
await client.Tweets.PublishTweetAsync(new PublishTweetParameters("hello")
{
    TweetMode = TweetMode.Extended
});
```

You can use `TweetMode.None` in a request parameters if you want to remove the compatibility for 1 specific rwequest.

### GetUtcDate

Requests sent to Twitter require a signature with the current utc date included.\
Signing a request with an invalid utc date will result in Twitter rejecting the request.\
By default Tweetinvi will use `Date.UtcNow`.

Some machines are not configured with the correct time and will result in an invalid value of `Date.UtcNow`.\
If the machine is intentionally misconfigured, you can override how tweetinvi gets the utc date time.

``` c#
client.Config.GetUtcDateTime = () =>
{
    // your custom logic to retrieve a utc date time
    return new DateTime();
};
```

### RateLimitTrackerMode

`RateLimitTrackerMode` gives you pre configured solution for managing rate limits in your applications.

To learn more about this config please [read more here](../credentials/rate-limits-handlers).

### RateLimitWaitFudge

When `RateLimitTrackerMode` is enabled, this sets an additional duration to wait after the reset has completed.\
Waiting for an additional duration help make sure that the reset has properly happened on the Twitter side.

``` c#
client.Config.RateLimitWaitFudge = TimeSpan.FromSeconds(2);
```

### Twitter Limits

TwitterLimits are used by the request validators to verify if a request you are attempting to perform is matching the limits imposed by Twitter.\
This can involve the number of items, the size of text or specific values for numbers.

Tweetinvi limits are set as per the documentation and these values might change in the future.\
If these values do change you have the ability to change the TwitterLimits.

``` c#
client.Config.Limits.MESSAGES_GET_MAX_PAGE_SIZE = 100;

try
{
    client.Messages.ParametersValidator.Validate(new GetMessagesParameters
    {
        PageSize = 101
    });
}
catch (TwitterArgumentLimitException e)
{
    // Argument parameters.PageSize was over the limit of 100 page size
    Console.WriteLine(e);
}
```

## Request Validators

Tweetinvi 5.0 introduced the concept of request validators.\
Their goal is to help developers verifying that requests parameters are valid.

The `Validate` method takes parameters and throws an `ArgumentException` if any property is incorrect.

``` c#
var parameters = new GetMessagesParameters();

try
{
    client.ParametersValidator.Validate(parameters);
    client.Messages.ParametersValidator.Validate(parameters);
}
catch (TwitterArgumentLimitException limitException)
{
}
catch (ArgumentException argumentException)
{
}
```

The `TwitterArgumentLimitException` are being thrown when a limit specified in the `client.Config.Limits` is not respected.\
This type of error will provide additional information as to why the limit was breached.

