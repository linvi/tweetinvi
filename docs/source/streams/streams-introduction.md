# Streams Introduction

Twitter API exposes multiple type of streams. In their public API you will have access to:

* The [Sample Stream](./sample-stream) is a stream of tweets returning 1% of the tweets published in the world randomly

## What is a stream?

A stream emit events that can be listened to. These events are triggered by messages received from Twitter.\
You can find a list of the available events at the [bottom of this page](#stream-events).

``` c#
var twitterStream = client.Streams.CreateTweetStream();
twitterStream.EventReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Json);
};
await twitterStream.StartAsync("https://stream.twitter.com/1.1/statuses/sample.json");
```

## Start, Pause and Stop

A stream can have 3 different states:

* `StreamState.Running` - the stream is actively running and events are triggered when messages are received from Twitter. Technically, a socket is open with Twitter server until the stream stops.
* `StreamState.Stop` - the stream is not running
* `StreamState.Pause` - the stream has been paused. The socket is still open but we are not consuming the messages sent by Twitter.

When you create a stream, no connection is yet established.\
You need to start the stream for it to open a connection and start receiving events.

When you are done with the current stream, you stop it.

``` c#
// Here is an example showing how to stop a stream. In this case we want to stop after receiving 5 events.

var i = 0;
var twitterStream = client.Streams.CreateTweetStream();
twitterStream.EventReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Json);
    if (i == 5)
    {
        twitterStream.Stop();
        Console.WriteLine("Complete!");
    }

    ++i;
};

// NOTE: this will complete only when `sampleStream.Stop();` has been called
await twitterStream.StartAsync("https://stream.twitter.com/1.1/statuses/sample.json"); 
```

### Should I use Pause() or Stop()?

The difference between a `Paused` stream and a `Stopped` stream is the socket connection.\
When a stream is stopped, no socket connection exist.\
When a stream is paused, a socket connection is opened but is not used.

Having an open socket allow to "restart" the stream instantaneously. If the stream was stopped a connection should first be established before starting to receive the events.

> Stop() should be used when you know that you will no longer need to use the stream.
> Stop() should be used when you do not care of losing events happening during the milliseconds of connecting to Twitter. 
> Pause() should be used only when you need to quickly switch between pause and running.

## Stream Events

Tweetinvi offers various events to help you manage your stream and easily make sense of messages received from Twitter.

### Twitter events

| Event                        | Description                                                                                                                                                                                                      |
|------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| EventReceived                | Indicates that an event was received from twitter                                                                                                                                                                |
| TweetDeleted                 | Indicates that a tweet was deleted on Twitter. If you store any content of that tweet in your system, you should delete it                                                                                       |
| TweetLocationInfoRemoved     | Indicates that the location information of a tweet was removed. If you store the location of the tweet in your system, you should delete it                                                                      |
| TweetWitheld                 | Indicates that a tweet was created but the event cannot be forwarded to your application as it has been blocked by your country                                                                                  |
| UserWitheld                  | Indicates that a user matching your stream criteria performed an operation but the event cannot be forwarded to your application as the user is blocked in your country                                          |
| LimitReached                 | Indicates that your stream criteria match more than 1% of the worlwide tweets. As a consequences some of the tweets will not be forwarded to the stream                                                          |
| WarningFallingBehindDetected | Indicates that your application is not consuming the messages from the stream fast enough. Failing to improve the speed of consumption will result in the stream being disconnected                              |
| UnmanagedEventReceived       | Indicates that an event was received but Tweetinvi does not yet support it. If you receive such event please [create an issue on github](https://github.com/linvi/tweetinvi/issues/new) with the `json` received |

### Stream state events

Here is a list of events supported by Tweetinvi.

**Twitter events**

| Event                     | Description                                                                                                                               |
|---------------------------|-------------------------------------------------------------------------------------------------------------------------------------------|
| DisconnectMessageReceived | Twitter informs you that you are being disconnecting. A reason will be provided as to why you are                                         |
| KeepAliveReceived         | Twitter emits keep alive events at regular intervals to inform that the socket is open and that if no events were received it is expected |

**Tweetinvi events**


| Event              | Description                                                                |
|--------------------|----------------------------------------------------------------------------|
| StreamStateChanged | The state of the stream has changed (between running, paused and stopped)  |
| StreamStarted      | The stream is started and will be emitting events when received            |
| StreamResumed      | The stream that was previously paused is now consuming events from Twitter |
| StreamPaused       | The stream is no longer consuming events sent by Twitter                   |
| StreamStopped      | The stream is no longer active. The connection has been closed             |


## Options

### Language Filters

It is possible to restrict the stream to only return content associated with a specific language.
You cannot change the language while a stream is running, this has to be done before calling `StartAsync`.

``` c#
// add a language
stream.AddLanguageFilter(LanguageFilter.French);
// or
stream.AddLanguageFilter("fr");

// list filters applied to your stream
var languages = stream.LanguageFilters;
```

### Filter levels

Twitter allow to filter the tweets that could have sensitive content using the `filter_level`.

You can read more about it here : https://developer.twitter.com/en/docs/tweets/filter-realtime/guides/basic-stream-parameters.

``` c#
stream.FilterLevel = StreamFilterLevel.Low;
```

### Stall Warnings

Twitter can send messages relevant to the stream state. To decide to not receive such messages, you can set the `stall_warnings`.

``` c#
stream.StallWarnings = false;
```

### Tweet Mode

`TweetMode` can be set to define the format of tweets messages received. [Learn more about TweetMode](../more/extended-tweets).

``` c#
sampleStream.TweetMode = TweetMode.Compat;
```

### Custom Query Parameters

Tweetinvi supports all the currently existing parameters from the stream API.\
If new query parameters appear in the future you can keep using Tweetinvi via the custom query parameters.

``` c#
stream.AddCustomQueryParameter("new_location_filter", "paris");
```
