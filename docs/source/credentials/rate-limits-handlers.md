# Rate Limits Handlers

Application credentials and User credentials can access a variety of endpoints.\
Each of these endpoints can be accessed a limited number of times.

> For example a set of user credentials can perform 180 searches in 15 minutes intervals. If you try to do more than 180 searches in the 15 minutes timeframe the REST API will raise an exception.


## Default Handler

By default, a `TwitterClient` do not track the rate limits.\
It means that when a endpoint is out of limits, executing a request on that endpoint will result in a `TwitterException`.

In this mode you will need to catch `TwitterException` to detect when the rate limits allowances have exhausted.

``` c#
try
{
    var tweetinviUser = await client.Users.GetUserAsync("tweetinviapi");
}
catch (TwitterException e) 
{
    if (e.StatusCode == 429) 
    {
        // Rate limits allowance have been exhausted - do your custom handling
    }
}
```



In this mode you will need to use the Rate Limits API.\
Using `GetRateLimitsAsync`, `GetEndpointRateLimitAsync` and `WaitForQueryRateLimitAsync` should help you handling the rate limits on your own.

<div class="note">

[Lear more about the Rate Limits API](./rate-limits)
</div>


## Track and Await Handler

When using the `TrackAndAwait` rate limits mode will automatically handle the time when requests are being invoked.

* Tweetinvi will regularly request twitter for the current state of the credentials rate limits.
* Tweetinvi will perform some offline tracking of the rate limits to optimise the number of calls to twitter.
* When performing a request:
    * If the allowance for a specific endpoint **is not empty**, Tweetinvi will run the request immediately.
    * If the allowance for a specific endpoint **is empty**, Tweetinvi will automatically wait and schedule when the operation will be executed.

``` c#
// enable track and await
client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
```

## Track Only Handler

When using the `TrackOnly` rate limits mode will automatically handle the time when requests are being invoked.\
`TrackOnly` works almost like `TrackAndAwait` apart from the fact that it will always execute requests immediately.

* Tweetinvi will regularly request twitter for the current state of the credentials rate limits.
* Tweetinvi will perform some offline tracking of the rate limits to optimise the number of calls to twitter.
* When performing a request:
    * If the allowance for a specific endpoint **is not empty**, Tweetinvi will run the request immediately.
    * **If the stock of requests for a specific endpoint **is empty**, Tweetinvi will run the request immediately.**

``` c#
// enable track only
client.Config.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;
```
