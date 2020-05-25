# Basic Concepts

Lets review the most commonly used functionality.



## TwitterClient

> The TwitterClient let you perform operations from a specific set of credentials.

``` c#
// Application client
var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");

// User client
var userClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
```

[Learn more about the the differences between app an user credentials](../credentials/credentials)

<div class="note">

The documentation will use the following rules regarding the credentials:

* `appClient` indicates that the endpoint requires Application Credentials (with or without bearer token)
* `userClient` indicates that the endpoint requires User Credentials
* `client` indicates that the endpoint requires either application or user credentials.

</div>

## Users

``` c#
// Get the authenticated user
var authenticatedUser = await userClient.Users.GetAuthenticatedUserAsync();

// Get a specific user
var user = await client.Users.GetUserAsync("tweetinviapi");
```

## Tweets

``` c#
// Publish a tweet
var publishedTweet = await userClient.Tweets.PublishTweetAsync("Hello Tweetinvi!");

// Get tweet by id
var tweet = await client.Tweets.GetTweetAsync(publishedTweet.Id);
```

## Timelines

``` c#
// Get the tweets available on the user's home page
var homeTimeline = await userClient.Timelines.GetHomeTimelineAsync();

// Get tweets from a specific user
var userTimeline = await client.Timelines.GetUserTimelineAsync("tweetinviapi");
```

## Iterators

> Tweetinvi offers a simple approach to paging endpoints. This is called iterator.

[Learn more about iterators](../twitter-api/iterators)

Find an example below with the user timeline iterator.

<div class="iterator-available">

``` c#
var userTimelineIterator = client.Timelines.GetUserTimelineIterator("tweetinviapi");

while (!userTimelineIterator.Completed)
{
    var page = await userTimelineIterator.NextPageAsync();
    Console.WriteLine("Retrieved " + page.Count() + " tweets!");
}

Console.WriteLine("We have now retrieved all the tweets!");
```
</div>

## Search

``` c#
// Search tweets
var tweets = await client.Search.SearchTweetsAsync("hello");

// Search users
var users = await userClient.Search.SearchUsersAsync("tweetinviapi");
```

## Parameters

Tweetinvi provide many overloads to support typical use cases.\
All methods also provide an overload with a parameters class with more options.

These parameters can be accessed in the namespace `using Tweetinvi.Parameters;`.

Here is an example:

``` c#
// simple search
var tweets = await client.Search.SearchTweetsAsync("hello");

// complex search
var frenchTweets = await client.Search.SearchTweetsAsync(new SearchTweetsParameters("hello")
{
    Lang = LanguageFilter.French
});
```

## Smart Objects

Some objects like tweets expose methods.\
When you use invoke such methods, the client which created the object will be used to perform the request.

``` c#
var tweet = await client.Tweets.GetTweetAsync(42);

// DestroyAsync will be invoked via client
// if client has app credentials, the operation will fail
// if client has the user credentials of the user who created the tweet, the operation will pass
await tweet.DestroyAsync();

// it is the same as executing
await userClient.Tweets.DestroyTweetAsync(tweet);
```

You can change the client of smart objects.

``` c#
var myNewClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
tweet.Client = myNewClient;

// DestroyAsync will be invoked via myNewClient
await tweet.DestroyAsync();
```