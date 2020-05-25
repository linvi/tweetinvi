# Users

## Get a user

``` c#
var tweetinviUser = await client.Users.GetUserAsync("tweetinviapi");
var tweetinviUser = await client.Users.GetUserAsync(1577389800);

// You can aslo get multiple users at once
var users = await client.Users.GetUsersAsync(new long[]{ 42, 1577389800 });
```

## Get the authenticated user

Get the user associated with the client's credentials. This user has additional fields and methods.

``` c#
var authenticatedUser = await userClient.Users.GetAuthenticatedUserAsync();
```

## Block

> Blocks the specified user from following the authenticating user. In addition the blocked user will not show in the authenticating users mentions or timeline (unless retweeted by another user). If a follow or friend relationship exists it is destroyed.

[Learn more](https://help.twitter.com/en/using-twitter/blocking-and-unblocking-accounts)

``` c#
await userClient.Users.BlockUserAsync(userId);
await userClient.Users.UnblockUserAsync(userId);
```

<div class="iterator-available">

``` c#
var blockedUserIds = await userClient.Users.GetBlockedUserIdsAsync();
// or
var blockedUserIdsIterator = userClient.Users.GetBlockedUserIdsIterator();
```
</div>
<div class="iterator-available">

``` c#
var blockedUsers = await userClient.Users.GetBlockedUsersAsync();
// or
var blockedUsersIterator = userClient.Users.GetBlockedUsersIterator();
```

</div>

## Mute

> Mute is a feature that allows you to remove an account's Tweets from your timeline without unfollowing or blocking that account.

[Learn more](https://help.twitter.com/en/using-twitter/twitter-mute)

``` c#
await userClient.Users.MuteUserAsync(userId);
await userClient.Users.UnmuteUserAsync(userId);
```

<div class="iterator-available">

``` c#
var mutedUserIds = await userClient.Users.GetMutedUserIdsAsync();
// or
var mutedUserIdsIterator = userClient.Users.GetMutedUserIdsIterator();
```

</div>
<div class="iterator-available">

``` c#
var mutedUsers = await userClient.Users.GetMutedUsersAsync();
// or
var mutedUsersIterator = userClient.Users.GetMutedUsersIterator();
```

</div>

You can also list the users whose retweets are muted.

``` c#
var usersWhoseRetweetsAreMuted = await userClient.Users.GetUserIdsWhoseRetweetsAreMutedAsync();
```

## Report

> Report the specified user as a spam account to Twitter. Additionally, it blocks the user on behalf of the authenticated user.

[Learn more](https://developer.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam)

``` c#
await userClient.Users.ReportUserForSpamAsync("");
```