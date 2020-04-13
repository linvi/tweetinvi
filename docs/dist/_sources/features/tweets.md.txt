# Tweets

## Create, Read, Delete

``` c#
var tweet = await client.Tweets.PublishTweet("My first tweet with Tweetinvi!");
var publishedTweet = await client.Tweets.GetTweet(tweet.Id);
await client.Tweets.DestroyTweet(tweet);
```

Tweets are not just text, here are [various additional metadata](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-update) you can add to your tweets.

``` c#
var fullTweet = await client.Tweets.PublishTweet(new PublishTweetParameters("A complex tweet from Tweetinvi")
{
    Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
    DisplayExactCoordinates = true,
    TrimUser = true,
    PlaceId = "3e8542a1e9f82870",
    PossiblySensitive = true,
});
```

> Tweets are created by default with `tweet_mode` set to `extended`. You can change this in all parameters associated with tweets.\
> [Learn more about extended mode](../more/extended-tweets)

## Retweets

``` c#
// publish a retweet
var retweet = await client.Tweets.PublishRetweet(tweet);

// destroy the retweet
await client.Tweets.DestroyRetweet(retweet);
```

<div class="iterator-available">

``` c#
// get retweeters
var retweeters = await client.Tweets.GetRetweeterIds();
// or
var retweeterIdsIterator = client.Tweets.GetRetweeterIdsIterator(tweet);
```

</div>

## Replies

``` c#
// reply to a tweet
var reply = await client.Tweets.PublishTweet(new PublishTweetParameters("here is a great reply")
{
    InReplyToTweet = tweet
});

// remove the same way as you would delete a tweet
await client.Tweets.DestroyTweet(reply);
```

## Publish with Media

You can attach images, gif and videos that you uploaded to your tweets.

> [Learn more on media upload](./upload-media)

### With an image

``` c#
var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
var uploadedImage = await client.Upload.UploadTweetImage(tweetinviLogoBinary);
var tweetWithImage = await client.Tweets.PublishTweet(new PublishTweetParameters("Tweet with an image")
{
    Medias = { uploadedImage }
});
```

### With a video

``` c#
var videoBinary = File.ReadAllBytes("./video.mp4");
var uploadedVideo = await client.Upload.UploadTweetVideo(videoBinary);

// IMPORTANT: you need to wait for Twitter to process the video
await client.Upload.WaitForMediaProcessingToGetAllMetadata(uploadedVideo);

var tweetWithVideo = await client.Tweets.PublishTweet(new PublishTweetParameters("tweet with media")
{
    Medias = { uploadedVideo }
});
```

## Favorites

``` c#
// favorite
await client.Tweets.FavoriteTweet(tweet);
// remove
await client.Tweets.UnfavoriteTweet(tweet);
```

<div class="iterator-available">

``` c#
// get user favourites
var favouritedTweets = await client.Tweets.GetUserFavoriteTweets("tweetinviapi");
// or
var favoriteTweetsIterator = client.Tweets.GetUserFavoriteTweetsIterator("tweetinviapi");
```

</div>

## OEmbed Tweets

You can generate oembed tweets from Tweetinvi. If you want to learn more please read [Twitter documentation](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed).

``` c#
var oEmbedTweet = await client.Tweets.GetOEmbedTweet(tweet);
```