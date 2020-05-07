# Direct API calls

Tweetinvi offers a simple approach to calling the Twitter api directly.\
Tweetinvi will take care of setting up the headers for you, you take care of the query!

## Example

``` c#
var homeTimelineResult = await client.Execute.RequestAsync(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

var jsonResponse = homeTimelineResult.RawResult;
```

You can also get objects.

``` c#
var result = await client.Execute.RequestAsync<TweetDTO[]>(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

// TweetDTO[]
var tweetDtos = result.DataTransferObject;

// ITweet[]
var tweets = client.Factories.CreateTweets(tweetDtos);
```

## Going further

Some requests (like the premium api) will require additional customization like setting the authentication.

``` c#
// Example using basic authentication

var username = "Your username";
var password = "Your password";
var basicAuthContent = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

var result = await client.Execute.RequestAsync(request =>
{
    request.Url = "https://....";
    request.CustomHeaders.Add("Authorization", "Basic " + basicAuthContent);
});
```