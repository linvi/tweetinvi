# Followers, Friends and Relationships

From a user account perspective another user can either be a follower or a person that you you follow also described as friend.

## Followers

``` c#
// follow a user
await client.Users.FollowUser("tweetinviapi");

// stop following a user
await client.Users.UnfollowUser(42);
```

### List followers

``` c#
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
var followers = await client.Users.GetFollowers("tweetinviapi");

// or

var followersIterator = client.Users.GetFollowersIterator(new GetFollowersParameters("tweetinviapi"));
while (!followersIterator.Completed)
{
    var followersPage = await followersIterator.MoveToNextPage();
}
```

## Followers Outgoing Requests

> As an authenticated user following a private user, an outgoing follower request is sent to this private user. 

``` c#
var userIdsYouRequestedToFollow = await client.Users.GetUserIdsYouRequestedToFollow();

// or

var userIdsYouRequestedToFollowIterator = client.Users.GetUserIdsYouRequestedToFollowIterator();
while (!userIdsYouRequestedToFollowIterator.Completed)
{
    var followerIdsRequestPage = await userIdsYouRequestedToFollowIterator.MoveToNextPage();
}
```

Tweetinvi can fetch populated user objects by performing a set of additional requests. This iterator can retrieve up to 100 users per page.

``` c#
var usersYouRequestedToFollow = await client.Users.GetUsersYouRequestedToFollow();

// or

var usersYouRequestedToFollowIterator = client.Users.GetUsersYouRequestedToFollowIterator();
while (!usersYouRequestedToFollowIterator.Completed)
{
    var followerRequestPage = await usersYouRequestedToFollowIterator.MoveToNextPage();
}
```

## Followers Incoming Requests

> As a private user, any other user attempting to follow you will create an incoming request.

``` c#
var userIdsRequestingFriendship = await client.Users.GetUserIdsRequestingFriendship();

// or

var userIdsRequestingFriendshipIterator = client.Users.GetUserIdsRequestingFriendshipIterator();
while (!userIdsRequestingFriendshipIterator.Completed)
{
    var userIdsPage = await userIdsRequestingFriendshipIterator.MoveToNextPage();
}
```

Tweetinvi can fetch populated user objects by performing a set of additional requests. This iterator can retrieve up to 100 users per page.

``` c#
var userRequestingFriendship = await client.Users.GetUsersRequestingFriendship();

// or

var usersRequestingFriendshipIterator = client.Users.GetUsersRequestingFriendshipIterator();
while (!usersRequestingFriendshipIterator.Completed)
{
    var userIdsPage = await usersRequestingFriendshipIterator.MoveToNextPage();
}
```

## Friends

> List the users followed by a specific user.

``` c#
var friendIds = await client.Users.GetFriendIds("tweetinviapi");

// or

var friendIdsIterator = client.Users.GetFriendIdsIterator(new GetFriendIdsParameters("tweetinviapi"));
while (!friendIdsIterator.Completed)
{
    var friendIdsPage = await friendIdsIterator.MoveToNextPage();
}
```

Tweetinvi can fetch populated user objects by performing a set of additional requests. This iterator can retrieve up to 100 users per page.

``` c#
var friends = await client.Users.GetFriends("tweetinviapi");

// or

var friendIdsIterator = client.Users.GetFriendIdsIterator(new GetFriendIdsParameters("tweetinviapi"));
while (!friendIdsIterator.Completed)
{
    var followersPage = await friendIdsIterator.MoveToNextPage();
}
```

## Relationships

> Details of relationships between 2 users.

``` c#
var relationship = await client.Users.GetRelationshipBetween("source_user", "target_user");
// You can for example check if you
var canSourceSendDirectMessagesToTarget = relationship.CanSendDirectMessage;
```

List the relationships between the client's authenticated user and a list of users.

``` c#
var relationships = await client.Users.GetRelationshipsWith(new [] { "target_user" });
var userSentAFollowingRequest = relationships[0].FollowingRequested;
```

Updated relationship state between the authenticated user and a target user.

``` c#
await client.Users.UpdateRelationship(new UpdateRelationshipParameters("target_user")
{
    EnableRetweets = true,
    EnableDeviceNotifications = true
});
```