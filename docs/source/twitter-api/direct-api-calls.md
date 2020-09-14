# Direct API calls

Tweetinvi offers a simple approach to calling the Twitter api directly.\
Tweetinvi will take care of setting up the headers for you, you take care of the query!

## Factories

In order to transform the response into a Tweetinvi object you can use the `TwitterClient.Factories` which contains tenth of helper methods to let you create any object you can think of. Factories support creating objects by parsing `JSON` blobs as well as creating objects from Data Transfer Objects (DTO).

## Execute

The `Execute` client let you pass any request url, http method and http content and take care of executing the request for you.

``` c#
var homeTimelineResult = await client.Execute.RequestAsync(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

var jsonResponse = homeTimelineResult.Content;
```

You can also get objects.

``` c#
var result = await client.Execute.RequestAsync<TweetDTO[]>(request =>
{
    request.Url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
    request.HttpMethod = HttpMethod.GET;
});

// TweetDTO[]
var tweetDtos = result.Model;

// ITweet[]
var tweets = client.Factories.CreateTweets(tweetDtos);
```

## Raw Clients

The `Raw clients` offer to developers a similar api as the `TwitterClient`. You pass parameters and Tweetinvi computes the query.\
The main difference is that you will receive the raw response from Twitter instead of a model.

``` c#
var twitterResult = await userClient.Raw.Users.GetAuthenticatedUserAsync(new GetAuthenticatedUserParameters());
var statusCode = twitterResult.Response.StatusCode;

var json = twitterResult.Content;
var authenticatedUser = userClient.Factories.CreateAuthenticatedUser(json);

// or
var userDTO = twitterResult.Model;
var authenticatedUser = userClient.Factories.CreateAuthenticatedUser(userDTO);
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