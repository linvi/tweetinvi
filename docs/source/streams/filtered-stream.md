# Filtered Streams

> Filtered stream return tweets matching a specific set of criteria. ([twitter doc](https://developer.twitter.com/en/docs/tweets/filter-realtime/api-reference/post-statuses-filter))\
> Filtered streams support 3 type of filters, track (text content), location and users.

<div class="available-filters" style="">
    <a href="./filtered-stream-filters" class="success">List of available filters
        <span class="fa fa-arrow-circle-right" style="margin-left: 10px; font-size: 17px;"></span>
    </a>
</div>

The sample stream supports all the generic stream functionalities. [Read more about streams](./streams-introduction).

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

`MatchingTweetReceived` event contains multiple information helping you understand why the event was triggered.

### MatchingTweetReceived - Properties

`MatchOn` - states which part of the tweet matched the filters criteria.\
`QuotedTweetMatchOn` - states which part of the quoted tweet matched the filters criteria.

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

`MatchingTracks` - contains all the tracks that the tweet matched.\
`MatchingFollowers` - contains all the user ids matching the followed users.\
`MatchingLocations` - contains all the locations that the tweet matched

### MatchingTweetReceived - MatchOn

Tweetinvi let you filter the results based on where the content was matched.\
The `stream.MatchOn` property creates a filter and the `MatchingTweetReceived` will only be triggered if this condition is met.

By default `stream.MatchOn` is set to `MatchOn.Everything`, which means that any tweet received regardless of the reason will trigger the `MatchingTweetReceived`.

``` c#
// configure the filtered stream to only trigger when a track was found in
// EITHER the text of tweet OR in the urls of the tweet
stream.MatchOn = MatchOn.TweetText | MatchOn.URLEntities;
```

## Start Stream

Filtered stream can be started in 2 modes, **ANY** or **ALL**.\
They define the conditions required for the `MatchingTweetReceived` event to be triggered.

|                    | ANY                                                                | ALL                                                                |
|--------------------|--------------------------------------------------------------------|--------------------------------------------------------------------|
| Trigger Conditions | A tweet matches **EITHER** a track **OR** a location **OR** a user | A tweet matches **BOTH** a track **AND** a location **AND** a user |
| Invoke With        | `await stream.StartMatchingAnyConditionAsync();`                   | `await stream.StartMatchingAllConditionsAsync();`                  |

**Table comparing Any and All behaviour**

``` c#
var stream = client.Streams.CreateFilteredStream();
stream.AddTrack("france");
stream.AddTrack("hello world");
stream.AddFollow(42);

// ANY
await stream.StartMatchingAnyConditionAsync();

// ALL
await stream.StartMatchingAllConditionsAsync();
```

<div id="table-any-vs-all">

| Tweet Text         | Published By | Matched by ANY | Matched by ALL | Explanation                                                                                                                                                                                                                                     |
|--------------------|--------------|----------------|----------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| candy              | UserId: 1    | NO             | NO             | **ANY and ALL** are not matched as "candy" is not tracked                                                                                                                                                                                       |
| hello              | UserId: 1    | NO             | NO             | **ANY and ALL** are not matched as "hello" alone is not tracked, the text also need to contain "world"                                                                                                                                          |
| hello world        | UserId: 1    | YES            | NO             | **ANY** matched as "hello" and "world" are tracked<br/><br/>**ALL** is not matched as user id is not 42                                                                                                                                         |
| france             | UserId: 1    | YES            | NO             | **ANY** matched as "france" is tracked<br/><br/>**ALL** is not matched as user id is not 42                                                                                                                                                     |
| hello world france | UserId: 1    | YES            | NO             | **ANY** matched as "hello world" and "france" are tracked<br/><br/>**ALL** is not matched as user id is not 42                                                                                                                                  |
| candy              | UserId: 42   | YES            | NO             | **ANY** matched as tweet comes from user 42<br/><br/>**ALL** is not matched as "candy" is not tracked                                                                                                                                           |
| hello              | UserId: 42   | YES            | NO             | **ANY** matched as tweet comes from user 42<br/><br/>**ALL** is not matched as "hello" alone is not tracked                                                                                                                                     |
| hello world        | UserId: 42   | YES            | YES            | **ANY** matched as it needed either "hello world" tracked **OR** the tweet to be published by user 42<br/><br/>**ALL** matched as "hello world" is tracked **AND** tweet was published by user 42                                               |
| france             | UserId: 42   | YES            | YES            | **ANY** matched as it needed either "france" tracked **OR** the tweet to be published by user 42<br/><br/>**ALL** matched as "france" is tracked **AND** tweet was published by user 42                                                         |
| hello world france | UserId: 42   | YES            | YES            | **ANY** matched as it needed either `("hello world" OR "france")` tracked **OR** the tweet to be published by user 42<br/><br/>**ALL** matched as it needed either `("hello world" OR "france")` tracked **AND** tweet was published by user 42 |

</div>

<script>
    $("#table-any-vs-all td:contains('YES')").css("background-color", "#00a900").css("color", "white")
    $("#table-any-vs-all td:contains('NO')").css("background-color", "#cc0000").css("color", "white")
</script>

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

`NonMatchingTweetReceived` event can be triggered when:

* Your `MatchOn` stream filters has prevented this tweet from being matched
* Tweetinvi library can perform additional filters that Twitter do not support.
* Tweetinvi not properly understanding your filters. If this happen please open a ticket.
</div>
