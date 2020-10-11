# Basics of API V2

Twitter API v2 has a lot of changes compared to API v1.1.\
This page explains the new concepts and how to work with these concepts with Tweetinvi.

## Fields

[Official Doc Link](https://developer.twitter.com/en/docs/twitter-api/fields)

> The Twitter API v2 allows to specify which fields a query should return.

<div class="note">

By default Tweetinvi will request all the fields that are not subject to change from the user context.\
It means all the fields apart from some metric fields (non_public_metrics, organic_metrics and promoted_metrics).
</div>

In Tweetinvi, you can add/remove fields by specifying them in the request parameters.\
Tweetinvi list the fields available for each type of request in an associated `ResponseFields` class.

``` c#
// For a TweetQuery, you can find the fields in TweetResponseFields
var tweetResponse = await client.TweetsV2.GetTweetAsync(new GetTweetV2Parameters(42)
{
    Expansions =
    {
        TweetResponseFields.Expansions.AuthorId,
        TweetResponseFields.Expansions.ReferencedTweetsId,
    },
    TweetFields =
    {
        TweetResponseFields.Tweet.Attachments,
        TweetResponseFields.Tweet.Entities
    },
    UserFields = TweetResponseFields.User.ALL
});

// For a User query
var userResponse = userClient.UsersV2.GetUserByNameAsync(new GetUserByNameV2Parameters("tweetinviapi")
{
    Expansions =
    {
        UserResponseFields.Expansions.PinnedTweetId,
    },
    TweetFields =
    {
        UserResponseFields.Tweet.Attachments,
        UserResponseFields.Tweet.Entities
    },
    UserFields = UserResponseFields.User.ALL
});
```

## Expansion and Responses

[Official Doc Link](https://developer.twitter.com/en/docs/twitter-api/expansions)

Twitter API v2 reponses differ from API v1.1. In API v2, the objects are expanded in the `"Includes"` property of a response.

For example a Tweet no longer have a `CreatedBy` field. It now has an `AuthorId`.\
In a single query, you can request to retrieve the `Author` of the tweet, this is called an expansion as we are expanding the tweet and retrieving additional objects.\
Twitter response will contain the author in the property `Include.Users`.

``` c#
var tweetResponse = await userClient.TweetsV2.GetTweetAsync(1313034609437880320);
var tweet = tweetResponse.Tweet;
var tweetAuthor = tweetResponse.Includes.Users[0];
```

The type of objects that can be expanded depend on the type of request, they include users, tweets, geo, polls...\
By default all the expansions are set. If needed, expansions can be set in the parameters. 

``` c#
var tweetResponse = await userClient.TweetsV2.GetTweetAsync(new GetTweetV2Parameters(1313034609437880320)
{
    Expansions =
    {
        TweetResponseFields.Expansions.AuthorId
    }
});
```

<div class="note">

Working with expansions can be difficult at the current stage. In future releases, Tweetinvi will offer:

* Objects to easily find the information like `Dictionary<TweetV2, UserV2> UsersByTweet`.
* Compacted Responses will reshape de data in new comprehensive models. Such model will move the expansions within the objects. For example having a property "Author" in a Tweet object.
</div>

## Errors

[Official Doc Link](https://developer.twitter.com/en/support/twitter-api/error-troubleshooting)

API v2 changed the way it takes care of errors. `TwitterException` are still raised when the request go wrong but successful responses can carry a different type of errors.\
These are errors imply that the main resource has been successfully retrieved but some expansions might have failed to be retrieved. 

Let's assume a "tweet A" referenced another "tweet B". "Tweet B" got deleted.

Making a request to get the "tweet A", the request will be successful but the "referenced_tweet" expansion will fail with an error like the following:

``` json
{
    "detail": "Could not find tweet with referenced_tweets.id: [1313138609168551937].",
    "title": "Not Found Error",
    "resource_type": "tweet",
    "parameter": "referenced_tweets.id",
    "value": "1313138609168551937",
    "type": "https://api.twitter.com/2/problems/resource-not-found"
}
```

These errors are listed in a collection that can be found in the "Errors" property of the response.

``` c#
var tweetResponse = await userClient.TweetsV2.GetTweetAsync(1313034609437880320);
var errors = tweetResponse.Errors;
```