# Tweet Stream

> The tweet stream track tweets from a specific url.

This is a generic version of stream in the case Twitter decides to create a new type of stream.\
Tweetinvi provides specific streams objects for each different streaming apis.

* [Sample Stream](./sample-stream)
* [Filtered Stream](./filtered-stream)

``` c#
var stream = client.Streams.CreateTweetStream();
stream.EventReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Json);
};
await stream.StartAsync("https://stream.twitter.com/1.1/statuses/sample.json");
```
