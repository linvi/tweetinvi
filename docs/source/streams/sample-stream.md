# Sample Stream

> The sample stream returns 1% of all the public tweets published on Twitter at any time. Twitter randomly selects one tweet for every 100 tweets and publishes this Tweet in the stream.

The sample stream supports all the [generic stream events](./streams#stream-events).\
The `SampleStream` introduces 1 new event: `TweetReceived`.

``` c#
var sampleStream = client.Streams.CreateSampleStream();
sampleStream.TweetReceived += (sender, eventArgs) =>
{
    Console.WriteLine(eventArgs.Tweet);
};

await sampleStream.StartStreamAsync();
```

