# Filtered Stream - Filters

Filtered streams allow to track tweets based on:

* The text content - [Track Filter](#track-filter)
* The user who published the tweet - [Follow Filter](#follow-filter)
* The location where the tweet was publushed - [Location Filter](#location-filter)

## Track Filter

> The track filters let you track the tweets containing specific keywords.

`.AddTrack` allow you to add a track filter to your stream.

``` c#
// Only get tweets that contain (the keyword "france")
stream.AddTrack("france");
```

``` c#
// Only get tweets that contain (both the keywords "hello" AND "world")
stream.AddTrack("hello world");
```

``` c#
// Only get tweets that contain: 
// (the keyword "france") 
// OR 
// (both the keywords "hello" AND "world")
stream.AddTrack("france");
stream.AddTrack("hello world");
```

``` c#
// Only get tweets that contain the hashtag #hello
// Tweets matched by such keyword will contain a hashtag #hello under `Tweet.Entities.HashTags`
stream.AddTrack("#hello");
```

``` c#
// Only track tweets that contain a mention to the user "tweetinviapi"
// Tweets matched by such keyword will contain a mention @tweetinviapi under `Tweet.Entities.UserMentions`
stream.AddTrack("@tweetinviapi");
```

``` c#
// Only track tweets that contain the currency "$USD"
// Tweets matched by such keyword will contain a symbol USD under `Tweet.Entities.Symbols`
stream.AddTrack("$USD");
```

``` c#
// Only track tweets that contain the currency "twitter.com"
// Tweets matched by such keyword will contain a url "https://twitter.com" under `Tweet.Entities.Urls`
stream.AddTrack("twitter.com");

// Note that twitter documentation recommends to track "twitter com" (without '.') instead of "twitter.com".
// Tracking "twitter.com" will not return all tweets containing twitter.com. 
// If you track "twitter com", you will need to manually filter by checking the url entities.
stream.AddTrack("twitter com");
```

Please read the [Twitter documentation](https://developer.twitter.com/en/docs/tweets/filter-realtime/guides/basic-stream-parameters#track) to learn more how to build your track requests.

## Follow Filter

> The follow filter let you track tweets mentioning or published by a specific user.

`.AddFollow` allow you to add a follow filter to your stream.

``` c#
// Add a filter with a user id
stream.AddFollow(783214);

// or with a user
var twitterUser = await client.Users.GetUserAsync("twitter");
stream.AddFollow(twitterUser);
```

Please read the [Twitter documentation](https://developer.twitter.com/en/docs/tweets/filter-realtime/guides/basic-stream-parameters#follow) to learn more how to build your follow requests.


## Location Filter

> The location filter let you track tweets created at a specific location.

`.AddLocation` allow you to add a location filter to your stream.

``` c#
// Add a filter with a location
var centerOfNewYork = new Location(new Coordinates(-74,40), new Coordinates(-73,41));
stream.AddLocation(centerOfNewYork);

// or with 2 coordinates
stream.AddLocation(new Coordinates(-74,40), new Coordinates(-73,41));
```

Please read the [Twitter documentation](https://developer.twitter.com/en/docs/tweets/filter-realtime/guides/basic-stream-parameters#locations) to learn more how to build your location requests.