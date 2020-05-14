# Labs Sample Stream

> The labs sample stream is still in preview is likely to become a replacement of the current sample stream in the future. ([twitter doc](https://developer.twitter.com/en/docs/labs/sampled-stream/overview))

Tweetinvi does not properly support labs yet.\
Here is a workaround to work with the labs sample stream v1.

``` c#
// You need to use ConsumerOnly credentials to run a sample stream from labs
var appCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET")
{
    BearerToken = "APP_BEARER_TOKEN"
};

var appClient = new TwitterClient(appCredentials);
```


``` c#
var stream = appClient.Streams.CreateTweetStream();
stream.StallWarnings = null; // this is required as this parameter does not exist for this endpoint

stream.EventReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Json);
};

await stream.StartAsync("https://api.twitter.com/labs/1/tweets/stream/sample");
```

<div class="warning">
Labs sample stream only support application credentials (with a bearer token).<br/>
<a href="../twitter-api/credentials.html#bearer-token">Learn more about Application Only Credentials</a>
</div>