# Basics of API V2

This page explain the basic concepts of Twitter API V2 and how Tweetinvi works.

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