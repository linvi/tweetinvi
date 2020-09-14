# Extended Tweets

In 2016, Twitter decided to change the way Tweets work and they decided to call them **Extended Tweets** 

These tweets are different because the length of the text no longer include the `@username` that was considered as part of the 140 characters of the old generation of Tweets. In addition to this, the link to attachments are also no longer part of the 140 characters size limit.

You can learn more about the changes of Extended Tweets at [https://dev.twitter.com/overview/api/upcoming-changes-to-tweets](https://dev.twitter.com/overview/api/upcoming-changes-to-tweets).

## Tweet Mode

Twitter introduced a `TweetMode` parameter that is either `compat` or `extended`. Wherever this parameter has been documented, you will be able to set its value from the request parameters. You can also change the value at a client level. By default the client's `TweetMode` is `extended`.

``` c#
// You can modify this setting per client
userClient.Config.TweetMode = TweetMode.Compat;

// or set it directly in a request parameter
var getTimelineParameters = new GetUserTimelineParameters("tweetinviapi")
{
    TweetMode = TweetMode.Extended
};
```

When set to `null`, Tweetinvi will not add the `tweet_mode` parameter to the request url.

In addition to this, the `auto_populate_reply_metadata` and `exclude_reply_user_ids` has been added to the `PublishTweetOptionalParameters` class.

## Extended Tweet Properties

`ITweet` includes new properties that you can use to extract the information from an extended tweet.

``` c#
var tweet = Tweet.PublishTweet("@tweetinviapi forever! pic.twitter.com/42");
var fullText = tweet.FullText; // @tweetinviapi forever!
var prefix_or_mentions = tweet.Prefix; // @tweetinviapi
var content = tweet.Text; // forever!
var suffix = tweet.Suffix; // pic.twitter.com/42
```

You can also access some other metadata like :

``` c#
// Contains the location of the text to display (content)
int[] tweet.DisplayTextRange; 

// Contains all the information specific to extended tweets
IExtendedTweet tweet.ExtendedTweet;  
```

## Tweet parts

To preview how a string will be divided you can use the new string extension method `TweetParts()`.

``` c#
string tweet = "@tweetinviapi Amazing!";
ITweetParts tweetParts = tweet.TweetParts();

string prefix = tweetParts.Prefix;
string content = tweetParts.Content;
int twitterLength = tweetParts.Content.TweetLength();
string[] mentions = tweetParts.Mentions;
```

**NOTE :** `TweetParts` are only to be used with text that is intended to be used with `tweet_mode` extended and in reply to another tweet.