# Users

## Get a user

Get a user by either screen name or by its id.

``` c#
var tweetinviUser = await client.Users.GetUser("tweetinviapi");
var tweetinviUser = await client.Users.GetUser(1577389800);
```

## Get the authenticated user

Get the user associated with the client's credentials. This user has additional fields and methods.

``` c#
var authenticatedUser = await client.Users.GetAuthenticatedUser();
```

## Followers

``` c#
// follow a user
await client.Users.FollowUser("tweetinviapi");

// stop following a user
await client.Users.UnfollowUser(42);
```

### Listing followers

By default twitter only support retrieving follower ids. This iterator can retrieve up to 5000 follower ids per page.

``` c#
// Get a list of followers
var followerIds = await client.Users.GetFollowerIds("tweetinviapi");
// or
var followerIdsIterator = client.Users.GetFollowerIdsIterator(new GetFollowerIdsParameters("tweetinviapi"));
while (!followerIdsIterator.Completed)
{
    var followersPage = await followerIdsIterator.MoveToNextPage();
}
```

Tweetinvi can fetch populated user objects by performing a set of additional requests. This iterator can retrieve up to 100 users per page.

``` c#
var followersIterator = client.Users.GetFollowersIterator(new GetFollowersParameters("tweetinviapi"));
while (!followersIterator.Completed)
{
    var followersPage = await followersIterator.MoveToNextPage();
}
```

## Friends

By default twitter only support retrieving friend ids. This iterator can retrieve up to 5000 follower ids per page.

``` c#
// Get a list of friends
var friends = await client.Users.GetFriends("tweetinviapi");
// or
var friendIdsIterator = client.Users.GetFriendIdsIterator(new GetFriendIdsParameters("tweetinviapi"));
while (!friendIdsIterator.Completed)
{
    var friendIdsPage = await friendIdsIterator.MoveToNextPage();
}
```

Tweetinvi can fetch populated user objects by performing a set of additional requests. This iterator can retrieve up to 100 users per page.

``` c#
var friendIdsIterator = client.Users.GetFriendIdsIterator(new GetFriendIdsParameters("tweetinviapi"));
while (!friendIdsIterator.Completed)
{
    var followersPage = await friendIdsIterator.MoveToNextPage();
}
```