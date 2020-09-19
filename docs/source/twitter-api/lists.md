# Lists

> Lists group users so that users can read feeds of tweets only coming from the list's users.

## Create, Read, Update, Delete

``` c#
var createdList = await userClient.Lists.CreateListAsync("LIST_NAME");

var list = await client.Lists.GetListAsync(createdList.Id);

var updatedList = await userClient.Lists.UpdateListAsync(new UpdateListParameters(list)
{
    Name = "NEW_NAME"
});

await userClient.Lists.DestroyListAsync(list);
```

## Get List Tweets

``` c#
var tweets = await userClient.Lists.GetTweetsFromListAsync(list);
```

## Memberships

> Members of a lists are users whose tweets will be part of the list feed

``` c#
// Add members
var updatedList = await userClient.Lists.AddMemberToListAsync(list, "tweetinviapi");
var updatedList = await userClient.Lists.AddMembersToListAsync(list, new [] { "tweetinviapi", "twitter" });

// Check if a user is a member
var isMember = await userClient.Lists.CheckIfUserIsMemberOfListAsync(list, "tweetinviapi");

// Remove members
await userClient.Lists.RemoveMemberFromListAsync(list, "twitter");
await userClient.Lists.RemoveMembersFromListAsync(list, new [] { "twitter", "facebook" });
```

<div class="iterator-available">

``` c#
// Get members of a list
var members = await userClient.Lists.GetMembersOfListAsync(lists[0]);

// Get lists the authenticated user is a member of
var lists = await userClient.Lists.GetAccountListMembershipsAsync();

// Get lists a user is a member of
var lists = await userClient.Lists.GetUserListMembershipsAsync("tweetinviapi");
```

</div>

## Subscriptions

> Subscribers are users following a list.

``` c#
// Subscribe to a list
await userClient.Lists.SubscribeToListAsync(list);

// Is Subscribed
var isSubscribed = await userClient.Lists.CheckIfUserIsSubscriberOfListAsync(list, user);

// Unsubscribe from a list
await userClient.Lists.UnsubscribeFromListAsync(list);
```

<div class="iterator-available">

``` c#
// Get the subscribers of a list
var subsribers = await userClient.Lists.GetListSubscribersAsync(lists[0])

// Get the lists the authenticated user is subscribed to
var accountListSubscriptions = await userClient.Lists.GetAccountListSubscriptionsAsync();

// Get the lists a user is subscribed to
var userListSubscriptions = await userClient.Lists.GetUserListSubscriptionsAsync(user);
```

</div>

## Ownerships

> Account can own lists that they manage.

<div class="iterator-available">

``` c#
// Get lists owned by the authenticated user
var lists = await userClient.Lists.GetListsOwnedByAccountAsync();

// Get lists owned by a specific user
var lists = await userClient.Lists.GetListsOwnedByUserAsync(authenticatedUser);
```

</div>