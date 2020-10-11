# Labs Filtered Stream

> The labs filtered stream is still in preview is likely to become a replacement of the current filter stream in the future. ([twitter doc](https://developer.twitter.com/en/docs/labs/filtered-stream/overview))

Tweetinvi does not properly support labs yet.\
Here is a workaround to work with the labs filtered stream v1.

``` c#
// You need to use ConsumerOnly credentials to run a sample stream from labs
var appCredentials = new ConsumerOnlyCredentials("CONSUMER_KEY", "CONSUMER_SECRET")
{
    BearerToken = "APP_BEARER_TOKEN"
};

var appClient = new TwitterClient(appCredentials);

var stream = appClient.Streams.CreateTweetStream();
stream.StallWarnings = null; // this is required as this parameter does not exist for this endpoint

stream.EventReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Json);
};

await stream.StartAsync("https://api.twitter.com/labs/1/tweets/stream/filter");
```

<div class="warning">
Labs filtered stream only support application credentials (with a bearer token).<br/>
<a href="../credentials/credentials.html#bearer-token">Learn more about Application Only Credentials</a>
</div>