# Basic Concepts

In this page we will review the most commonly used functionality.

## TwitterClient

> The TwitterClient let you perform operations from a specific set of credentials.

``` c#
// Application client
var appClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET");

// User client
var userClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
```

## Users

``` c#
// Get the authenticated user
var authenticatedUser = await client.Users.GetAuthenticatedUser();

// Get a specific user
var user = await client.Users.GetUser("tweetinviapi");
```

## Tweets

``` c#
// Publish a tweet
var publishedTweet = await client.Tweets.PublishTweet("Hello Tweetinvi!");

// Get tweet by id
var tweet = await client.Tweets.GetTweet(publishedTweet.Id);
```

## Timelines

``` c#
// Get the tweets available on the user's home page
var homeTimeline = await client.Timelines.GetHomeTimeline();

// Get tweets from a specific user
var userTimeline = await client.Timelines.GetUserTimeline("tweetinviapi");
```

## Iterators

> Tweetinvi offers a simple approach to paging endpoints. This is called iterator.

Find an example below with the user timeline iterator.

``` c#
var userTimelineIterator = client.Timelines.GetUserTimelineIterator("tweetinviapi");

while (!userTimelineIterator.Completed)
{
    var page = await userTimelineIterator.MoveToNextPage();
    Console.WriteLine("Retrieved " + page.Count() + " tweets!");
}

Console.WriteLine("We have now retrieved all the tweets!");
```

## Search

``` c#
// Search tweets
var tweets = await client.Search.SearchTweets("hello");

// Search users
var users = await client.Search.SearchUsers("tweetinviapi");
```

## Parameters

Tweetinvi provide many overloads to support typical use cases.\
All methods also provide an overload with a parameters class with more options.

These parameters can be accessed in the namespace `using Tweetinvi.Parameters;`.

Here is an example:

``` c#
// simple search
var tweets = await client.Search.SearchTweets("hello");

// complex search
var frenchTweets = await client.Search.SearchTweets(new SearchTweetsParameters("hello")
{
    Lang = LanguageFilter.French
});
```