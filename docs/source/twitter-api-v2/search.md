# Search V2

## Search Tweets

Searching for tweets tweets in Twitter API v2 support both application and user credentials.\
Note that the value of some properties might differ between the 2 types of authentication.

``` c#
var searchResponse = await userClient.SearchV2.SearchTweetsAsync("hello");
var tweets = searchResponse.Tweets;
```

The iterator for API v2 works slightly differently for API V2 as responses not only include the tweets but also expansions.

``` c#
var searchIterator = userClient.SearchV2.GetSearchTweetsV2Iterator("hello");
while (!searchIterator.Completed)
{
    var searchPage = await searchIterator.NextPageAsync();
    var searchResponse = searchPage.Content;
    var tweets = searchResponse.Tweets;
    // ...
}
```