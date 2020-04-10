# Tweets

## Create, Read, Delete

``` c#
var tweet = await client.Tweets.PublishTweet("My first tweet with Tweetinvi!");
var publishedTweet = await client.Tweets.GetTweet(tweet.Id);
await client.Tweets.DestroyTweet(tweet);
```

Tweets are not just text, here are [various additional metadata](https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-update) that you add to your tweets.

``` c#
var fullTweet = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("A complex tweet from Tweetinvi")
{
    Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
    DisplayExactCoordinates = true,
    TrimUser = true,
    PlaceId = "3e8542a1e9f82870",
    PossiblySensitive = true,
});
```

## Reply to a tweet

<div class="warning">
// TODO
</div>

## Publish with Media

You can attach images, gif and videos that you uploaded to your tweets.

> [Learn more on media upload](./media)

### With an image

``` c#
var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
var tweetWithImage = await client.Tweets.PublishTweet(new PublishTweetParameters("Tweet with an image")
{
    MediaBinaries = { tweetinviLogoBinary }
});
```

### With a video

``` c#
var videoBinary = File.ReadAllBytes("./video.mp4");

var uploadedVideo = await _protectedClient.Upload.UploadVideo(tweetinviLogoBinary);
await _protectedClient.Upload.WaitForMediaProcessingToGetAllMetadata(uploadedVideo);

var tweetWithVideo = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("tweet with media")
{
    Medias = { uploadedVideo }
});
```