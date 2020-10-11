# Sample Stream V2

> The sampled stream endpoint delivers a roughly 1% random sample of publicly available Tweets in real-time. With it, you can identify and track trends, monitor general sentiment, monitor global events, and much more.

``` c#
var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
sampleStreamV2.TweetReceived += (sender, args) =>
{
    System.Console.WriteLine(args.Tweet.Text);
};

await sampleStreamV2.StartAsync();
```