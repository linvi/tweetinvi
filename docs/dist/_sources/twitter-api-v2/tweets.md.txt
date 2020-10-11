# Tweets V2

## Get Tweets

Retrieving tweets from the Twitter API v2 support both application and user credentials.\
Note that the value of some properties might differ between the 2 types of authentication.

``` c#
// single tweet
var tweetResponse = await client.TweetsV2.GetTweetAsync(1313034609437880320);
var tweet = tweetResponse.Tweet;

// multiple tweets
var tweetsResponse = await client.TweetsV2.GetTweetsAsync(1313034609437880320, 1312922108993957888);
var tweets = tweetsResponse.Tweets;
```

[Expansions and custom fields](./basics) can be found in the class `TweetResponseFields`.

## Hide Replies

A user can hide/unhide tweets that were published to reply to a tweet he published.

``` c#
var tweetId = 42;
var hideResponse = await userClient.TweetsV2.ChangeTweetReplyVisibilityAsync(tweetId, TweetReplyVisibility.Hidden);
var isRepliedNowHidden = hideResponse.TweetHideState.Hidden;
```