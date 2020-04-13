# Users

## Get a user

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

## Block

> Blocks the specified user from following the authenticating user. In addition the blocked user will not show in the authenticating users mentions or timeline (unless retweeted by another user). If a follow or friend relationship exists it is destroyed.

[Learn more](https://help.twitter.com/en/using-twitter/blocking-and-unblocking-accounts)

``` c#
await client.Users.BlockUser(userId);
await client.Users.UnblockUser(userId);
```

<div class="iterator-available">

``` c#
var blockedUserIds = await client.Users.GetBlockedUserIds();
// or
var blockedUserIdsIterator = client.Users.GetBlockedUserIdsIterator();
```
</div>
<div class="iterator-available">

``` c#
var blockedUsers = await client.Users.GetBlockedUsers();
// or
var blockedUsersIterator = client.Users.GetBlockedUsersIterator();
```

</div>

## Mute

> Mute is a feature that allows you to remove an account's Tweets from your timeline without unfollowing or blocking that account.

[Learn more](https://help.twitter.com/en/using-twitter/twitter-mute)

``` c#
await client.Users.MuteUser(userId);
await client.Users.UnmuteUser(userId);
```

<div class="iterator-available">

``` c#
var mutedUserIds = await client.Users.GetMutedUserIds();
// or
var mutedUserIdsIterator = client.Users.GetMutedUserIdsIterator();
```

</div>
<div class="iterator-available">

``` c#
var mutedUsers = await client.Users.GetMutedUsers();
// or
var mutedUsersIterator = client.Users.GetMutedUsersIterator();
```

</div>

You can also list the users whose retweets are muted.

``` c#
var usersWhoseRetweetsAreMuted = await client.Users.GetUserIdsWhoseRetweetsAreMuted();
```

## Report

> Report the specified user as a spam account to Twitter. Additionally, it blocks the user on behalf of the authenticated user.

[Learn more](https://developer.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam)

``` c#
await client.Users.ReportUserForSpam("");
```