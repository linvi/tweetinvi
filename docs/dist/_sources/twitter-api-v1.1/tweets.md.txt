# Tweets

## Create, Read, Delete

``` c#
var tweet = await userClient.Tweets.PublishTweetAsync("My first tweet with Tweetinvi!");
var publishedTweet = await client.Tweets.GetTweetAsync(tweet.Id);
await userClient.Tweets.DestroyTweetAsync(tweet);
```

Tweets are not just text, here are [various additional metadata](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-update) you can add to your tweets.

``` c#
var fullTweet = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters("A complex tweet from Tweetinvi")
{
    Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
    DisplayExactCoordinates = true,
    TrimUser = true,
    PlaceId = "3e8542a1e9f82870",
    PossiblySensitive = true,
});
```

## Extended Tweets

> Tweets are created by default with `tweet_mode` set to `extended`. You can change this in all parameters associated with tweets.\
> [Learn more about extended mode](../more/extended-tweets)

## Retweets

``` c#
// publish a retweet
var retweet = await userClient.Tweets.PublishRetweetAsync(tweet);

// destroy the retweet
await userClient.Tweets.DestroyRetweetAsync(retweet);
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
var reply = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters("here is a great reply")
{
    InReplyToTweet = tweet
});

// remove the same way as you would delete a tweet
await userClient.Tweets.DestroyTweetAsync(reply);
```

## Publish with Media

You can attach images, gif and videos that you uploaded to your tweets.

> [Learn more on media upload](./upload-media)

### With an image

``` c#
var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
var uploadedImage = await userClient.Upload.UploadTweetImageAsync(tweetinviLogoBinary);
var tweetWithImage = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters("Tweet with an image")
{
    Medias = { uploadedImage }
});
```

### With a video

``` c#
var videoBinary = File.ReadAllBytes("./video.mp4");
var uploadedVideo = await userClient.Upload.UploadTweetVideoAsync(videoBinary);

// IMPORTANT: you need to wait for Twitter to process the video
await userClient.Upload.WaitForMediaProcessingToGetAllMetadataAsync(uploadedVideo);

var tweetWithVideo = await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters("tweet with media")
{
    Medias = { uploadedVideo }
});
```

## Favorites

``` c#
// favorite
await userClient.Tweets.FavoriteTweetAsync(tweet);
// remove
await userClient.Tweets.UnfavoriteTweetAsync(tweet);
```

<div class="iterator-available">

``` c#
// get user favourites
var favouritedTweets = await userClient.Tweets.GetUserFavoriteTweetsAsync("tweetinviapi");
// or
var favoriteTweetsIterator = userClient.Tweets.GetUserFavoriteTweetsIterator("tweetinviapi");
```

</div>

## OEmbed Tweets

> Twitter can generate HTML for you so that you can display a tweet on your website as it would appear on twitter.

You can generate oembed tweets from Tweetinvi. If you want to learn more please read [Twitter documentation](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed).

``` c#
var oEmbedTweet = await client.Tweets.GetOEmbedTweetAsync(tweet);
```