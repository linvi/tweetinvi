# Direct API calls

Tweetinvi offers a simple approach to calling the Twitter api directly.\
Tweetinvi will take care of setting up the headers for you, you take care of the query!

## Example

``` c#
var homeTimelineResult = await client.Execute.Request(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

var jsonResponse = result.RawResult;
```

You can also get objects.

``` c#
var result = await client.Execute.Request<TweetDTO[]>(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

// TweetDTO[]
var tweetDtos = result.DataTransferObject;

// ITweet[]
var tweets = client.Factories.CreateTweets(tweetDtos);
```