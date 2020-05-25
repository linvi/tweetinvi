# Rate Limits API

Application credentials and User credentials can access a variety of endpoints.\
Each of these endpoints can be accessed a limited number of times.

> For example a set of user credentials can perform 180 searches in 15 minutes intervals. If you try to do more than 180 searches in the 15 minutes timeframe the REST API will raise an exception.


<div class="note">

Tweetinvi offers 3 levels of support to manage RateLimits.\
These rate limits handlers let you focus on your application and reduce the need to use the Rate Limits API directly.

[Learn more about Rate Limits Handlers](./rate-limits-handlers)
</div>

## Rate Limits Cache

Rate limits are cached **locally** by Tweetinvi.\
When using the `TrackOnly` or `TrackAndAwait` tweetinvi will cache the rate limits allowances to reduce the number of calls performed to the twitter api.

The rate limits operations offer a `From` of type `RateLimitSource` that let you define if your rate limit operations will be performed from the cache or from the twitter api.

You can decide to clear your cache at any point of time.

``` c#
await client.RateLimits.ClearRateLimitCacheAsync();
```

## Get Rate Limits

`RateLimits.GetRateLimitsAsync()` retrieves the state of rßate limit allowances at a specific point of time.

``` c#
var rateLimits = await client.RateLimits.GetRateLimitsAsync();
var searchTweetsLimits = rateLimits.SearchTweetsLimit;
```

`RateLimits` contain the following information

| Property                    | Description                                                                                                                             |
|-----------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|
| Limit                       | Maximum number of requests available                                                                                                    |
| Remaining                   | Number of requests that can be performed before the reset. <br/>When this value hits 0, you cannot perform this action until the reset. |
| ResetDateTime               | DateTime of when the rate limits allowance will be reset                                                                                |
| ResetDateTimeInMilliseconds | Number of milliseconds before the rate limits allowance is reset                                                                        |
| ResetDateTimeInSeconds      | Number of seconds before the rate limits allowance is reset                                                                             |

## Get url endpoint allowance

`RateLimits.GetEndpointRateLimitAsync()` retrieves the rate limits allowances of a specific endpoint

``` c#
var url = "https://api.twitter.com/1.1/users/show.json?screen_name=tweetinviapi";

// get the endpoint rate limits
var endpointRateLimit = await client.RateLimits.GetEndpointRateLimitAsync(url);

// you can specify which source to use to retrieve the rate limits

var endpointRateLimit = await client.RateLimits.GetEndpointRateLimitAsync(url, RateLimitsSource.CacheOrTwitterApi);
```

<div class="note">

Note that rate limits for Application credentials are different than for User credentials.\
As some endpoints are not accessible through application credentials, attempting to access it will return null.
</div>


## Wait for endpoint allowance

`RateLimits.WaitForQueryRateLimitAsync()` waits the necessary time for the rate limits allowances to become available.

* If the allowance for a specific endpoint **is not empty**, `WaitForQueryRateLimitAsync` will complete immediately.
* If the allowance for a specific endpoint **is empty**, `WaitForQueryRateLimitAsync` will wait for the allowance to reset.

``` c#
var url = "https://api.twitter.com/1.1/users/show.json?screen_name=tweetinviapi";

// waits if no allowance is available for the get user endpoint
await client.RateLimits.WaitForQueryRateLimitAsync(url);

// you can specify which source to use to retrieve the rate limits
await client.RateLimits.WaitForQueryRateLimitAsync(url, RateLimitsSource.CacheOrTwitterApi);
```

## Rate Limits - Special Endpoints

For some endpoint Twitter has other type of rate limits. They usually apply rules to prevent bots.\
These rules are not strict and can be dependent on the way an account is used.\
For example you are likely to be stopped publishing tweets if you do publish more than 300 tweets within 3 hours.

Other rules apply and I suggest you check Twitter documentation to learn about them.

[https://developer.twitter.com/en/docs/basics/rate-limits](https://developer.twitter.com/en/docs/basics/rate-limits)