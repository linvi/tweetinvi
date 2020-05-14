# Filtered Streams

> Filtered stream return tweets matching a specific set of criteria. ([twitter doc](https://developer.twitter.com/en/docs/tweets/filter-realtime/api-reference/post-statuses-filter))\
> Filtered streams support 3 type of filters, track (text content), location and users.

<div class="available-filters" style="">
    <a href="./filtered-stream-filters" class="success">List of available filters
        <span class="fa fa-arrow-circle-right" style="margin-left: 10px; font-size: 17px;"></span>
    </a>
</div>

<style>
.available-filters {
    margin: 30px 0 30px 0;
}

@media only screen and (min-width: 1200px) {
    .available-filters {
        position: relative;
        margin: 0;
    }
    
    .available-filters a {
        position: absolute; 
        top: -75px; 
        right: 0;
    }
}
</style>

``` c#
// Create a simple stream containing only tweets with the keyword France

var stream = client.Streams.CreateFilteredStream();
stream.AddTrack("france");

stream.MatchingTweetReceived += (sender, eventReceived) =>
{
    Console.WriteLine(eventReceived.Tweet);
};

await stream.StartMatchingAnyConditionAsync();
```



## MatchingTweetReceived

`MatchingTweetReceived` event contains multiple properties helping you understand why the event was triggered.

`MatchOn` - states which part of the tweet matched the criteria. 

| MatchOn             | Description                                                                                    |
|---------------------|------------------------------------------------------------------------------------------------|
| TweetText           | The text of the tweet matched at least one of the stream track filters                         |
| Follower            | The user who created the tweet matched at least one of the stream follow filters               |
| TweetLocation       | The location where the tweet was published matched at least one of the stream location filters |
| FollowerInReplyTo   | The tweet was published to a user matching one of the stream follow filters                    |
|                     |                                                                                                |
| AllEntities         | Any of the entities matches one of the stream track filters                                    |
| URLEntities         | At least one of the tweet url entity matches one of the stream track filters                   |
| HashTagEntities     | At least one of the tweet hashtag entity matches one of the stream track filters               |
| UserMentionEntities | At least one of the tweet user mention entity matches one of the stream track filters          |
| SymbolEntities      | At least one of the tweet symbol entity matches one of the stream track filters                |

## Events per filter

`MatchingTweetReceived` allow you to find out when a tweet matching your filters criteria was received.\
When creating a filter you can associate a callback that will be invoked when this filter is being matched.

``` c#
stream.AddTrack("hello", tweet =>
{
    // A tweet containing 'hello' was found!
});

stream.AddTrack("bonjour", tweet =>
{
    // A tweet containing 'bonjour' was found!
});

stream.MatchingTweetReceived += (sender, eventReceived) =>
{
    // A tweet containing EITHER 'hello' OR 'bonjour' was found
};
```

## Events

The filtered stream supports all the [generic stream events](./streams-introduction#stream-events).

| Event                    | Description                                                                                 |
|--------------------------|---------------------------------------------------------------------------------------------|
| MatchingTweetReceived    | Indicates that a tweet matching the criteria you specified was created                      |
| NonMatchingTweetReceived | Indicates that a tweet that is **not** matching the criteria was received (read more below) |

<div class="warning">

NonMatchingTweetReceived can happen as the Tweetinvi library can perform additional filters that Twitter do not support.\
This behaviour could also be caused by Tweetinvi not properly understanding your filters. If this happen please open a ticket.

</div>
