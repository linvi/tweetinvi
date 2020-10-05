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