# Users

## Get a user

Get a user by either screen name or by its id.

``` c#
var tweetinviUser = await client.Users.GetUser("tweetinviapi");
var tweetinviUser = await client.Users.GetUser(1577389800);

// You can aslo get multiple users at once
var users = await client.Users.GetUsers(new long[]{ 42, 1577389800 });
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

### Follower Pending Requests

When a user request to follow another private users, this operation needs to be authorized by the private user.\
The following contains various methods allowing to check this.

<div class="warning">
TODO
</div>


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

## Block

``` c#
await client.Users.BlockUser(userId);
await client.Users.UnblockUser(userId);
```

``` c#
var blockedUserIds = await client.Users.GetBlockedUserIds();
// or
var blockedUserIdsIterator = client.Users.GetBlockedUserIdsIterator();
while (!blockedUserIdsIterator.Completed)
{
    var blockedUserIdsPage = await blockedUserIdsIterator.MoveToNextPage();
}

var blockedUsers = await client.Users.GetBlockedUsers();
// or
var blockedUsersIterator = client.Users.GetBlockedUsersIterator();
while (!blockedUsersIterator.Completed)
{
    var blockedUsersPage = await blockedUsersIterator.MoveToNextPage();
}
```

## Mute

``` c#
await client.Users.MuteUser(userId);
await client.Users.UnmuteUser(userId);
```

``` c#
var mutedUserIds = await client.Users.GetMutedUserIds();
// or
var mutedUserIdsIterator = client.Users.GetMutedUserIdsIterator();
while (!mutedUserIdsIterator.Completed)
{
    var mutedUserIdsPage = await mutedUserIdsIterator.MoveToNextPage();
}

var mutedUsers = await client.Users.GetMutedUsers();
// or
var mutedUsersIterator = client.Users.GetMutedUsersIterator();
while (!mutedUsersIterator.Completed)
{
    var mutedUsersPage = await mutedUsersIterator.MoveToNextPage();
}
```

You can also list the users whose retweets are muted.

``` c#
var usersWhoseRetweetsAreMuted = await client.Users.GetUserIdsWhoseRetweetsAreMuted();
```

## Report

``` c#
await client.Users.ReportUserForSpam("tweetinviapi");
```