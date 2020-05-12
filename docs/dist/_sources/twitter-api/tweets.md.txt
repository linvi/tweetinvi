# Tweets

## Create, Read, Delete

``` c#
var tweet = await client.Tweets.PublishTweetAsync("My first tweet with Tweetinvi!");
var publishedTweet = await client.Tweets.GetTweetAsync(tweet.Id);
await client.Tweets.DestroyTweetAsync(tweet);
```

Tweets are not just text, here are [various additional metadata](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-update) you can add to your tweets.

``` c#
var fullTweet = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("A complex tweet from Tweetinvi")
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
var retweet = await client.Tweets.PublishRetweetAsync(tweet);

// destroy the retweet
await client.Tweets.DestroyRetweetAsync(retweet);
```

<div class="iterator-available">

``` c#
// get retweeters
var retweeters = await client.Tweets.GetRetweeterIdsAsync(tweet);
// or
var retweeterIdsIterator = client.Tweets.GetRetweeterIdsIterator(tweet);
```

</div>

## Replies

``` c#
// reply to a tweet
var reply = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("here is a great reply")
{
    InReplyToTweet = tweet
});

// remove the same way as you would delete a tweet
await client.Tweets.DestroyTweetAsync(reply);
```

## Publish with Media

You can attach images, gif and videos that you uploaded to your tweets.

> [Learn more on media upload](./upload-media)

### With an image

``` c#
var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
var uploadedImage = await client.Upload.UploadTweetImageAsync(tweetinviLogoBinary);
var tweetWithImage = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("Tweet with an image")
{
    Medias = { uploadedImage }
});
```

### With a video

``` c#
var videoBinary = File.ReadAllBytes("./video.mp4");
var uploadedVideo = await client.Upload.UploadTweetVideoAsync(videoBinary);

// IMPORTANT: you need to wait for Twitter to process the video
await client.Upload.WaitForMediaProcessingToGetAllMetadataAsync(uploadedVideo);

var tweetWithVideo = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("tweet with media")
{
    Medias = { uploadedVideo }
});
```

## Favorites

``` c#
// favorite
await client.Tweets.FavoriteTweetAsync(tweet);
// remove
await client.Tweets.UnfavoriteTweetAsync(tweet);
```

<div class="iterator-available">

``` c#
// get user favourites
var favouritedTweets = await client.Tweets.GetUserFavoriteTweetsAsync("tweetinviapi");
// or
var favoriteTweetsIterator = client.Tweets.GetUserFavoriteTweetsIterator("tweetinviapi");
```

</div>

## OEmbed Tweets

You can generate oembed tweets from Tweetinvi. If you want to learn more please read [Twitter documentation](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed).

``` c#
var oEmbedTweet = await client.Tweets.GetOEmbedTweetAsync(tweet);
```