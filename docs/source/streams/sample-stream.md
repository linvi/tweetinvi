# Sample Stream

> The sample stream returns 1% of all the public tweets published on Twitter at any time. Twitter randomly selects one tweet for every 100 tweets and publishes this Tweet in the stream. ([twitter doc](https://developer.twitter.com/en/docs/tweets/sample-realtime/overview/get_statuses_sample))

The sample stream supports all the generic stream functionalities. [Read more about streams](./streams-introduction).

``` c#
var sampleStream = client.Streams.CreateSampleStream();
sampleStream.TweetReceived += (sender, eventArgs) =>
{
    Console.WriteLine(eventArgs.Tweet);
};

await sampleStream.StartAsync();
```

## Events

The sample stream supports all the [generic stream events](./streams-introduction#stream-events).

| Event         | TweetReceived                      |
|---------------|------------------------------------|
| TweetReceived | Indicates that a tweet was created |
