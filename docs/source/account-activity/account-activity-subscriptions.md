# Account Activity - Subscriptions

Once you have a [regitered your webhook](./account-activity#register-a-twitter-webhook) server, we can start receiving events for specific users.

In the examples below we will subscribe to the sandbox environment.

## Subscribe a user

Lets assume the `userClient` is associated with the twitter user `bob42`.

``` c#
await userClient.AccountActivity.SubscribeToAccountActivityAsync("sandbox");
```

The webhook will now receive all the events related to the `bob42` user.

<div class="warning">

**IMPORTANT -**
It takes some time for twitter to save the changes and you might not receive events straight away.\
You can expect to receive events at most after 30 seconds.
</div>

To unsubscribe a user.

``` c#
await userClient.AccountActivity.UnsubscribeFromAccountActivityAsync("sandbox", USER_ID);
```

## Check subscriptions

You can verify users subscribed to your webhook environment.\
All activities of a subscribed user will be notified to your webhook application.

``` c#
// requires application credentials
var environmentState = await appClient.AccountActivity.GetAccountActivitySubscriptionsAsync("sandbox");
var subscriptions = environmentState.Subscriptions;

// Get the user ids subscribed to your webhook
var userId = subscriptions[0].UserId;
```

Check if a specific user is subscribed to your webhook environment.

``` c#
// check if the user associated with `userClient` is subscribed to the sandbox environment
var subscribed = await userClient.AccountActivity.IsAccountSubscribedToAccountActivityAsync("sandbox");
```

The number of users who can be simultaneously connected to a webhook is limited and depends of your Twitter plan.\
To verify if you are going to reach the limit you can use the count endpoint.

``` c#
var subscription = await userClient.AccountActivity.CountAccountActivitySubscriptionsAsync();
var current = subscription.SubscriptionsCount;
var maxAvailable = subscription.ProvisionedCount;
```