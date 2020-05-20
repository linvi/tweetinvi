# Twitter Client - Events

Events are here to help developers manage the lifecycle of requests but also for logging.\
Each client will trigger its own events.


| Event                             | Description                                                                                                                                                                                                                             |
|-----------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| BeforeWaitingForRequestRateLimits | 1st event to be triggered. It is triggered as soon as a request is received.<br/>If you use a [Rate Limit handler](../credentials/rate-limits-handler), this event will be invoked before the handler has been invoked.                 |
| BeforeExecutingRequest            | 2nd event. It informs that the request is about to be performed.<br/>If you use a [Rate Limit Handlers](../credentials/rate-limits-handler), this event will be invoked after the necessary rate limits allowance has become available. |
| AfterExecutingRequest             | 3rd event. It informs that the request has completed (successfully or not).                                                                                                                                                             |
| OnTwitterException                | 4th event. It informs that the request resulted in an exception.                                                                                                                                                                        |

## Client level events

To receive client events, you just need to subscribe to them.

``` c#
var client = new TwitterClient(creds);

client.Events.BeforeExecutingRequest += (sender, args) =>
{
    // lets delay all operations from this client by 1 second
    Task.Delay(TimeSpan.FromSeconds(1));
};
```

## Application level events

If you wish to monitor the multiple clients or your entire application you can use the static `TweetinviEvents` as below.

``` c#
// subscribe to application level events
TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
{
    // application level logging
    Console.WriteLine(args.Url);
};

// For a client to be included in the application events you will need to subscribe to this client's events
TweetinviEvents.SubscribeToClientEvents(client);
```

<div class="warning">

Remember to unsubscribe your clients from your `TweetinviEvents` when you are done with them.\
Not doing so will result in a memory pointer still in use and will prevent the Garbage Collector to collect the client.

``` c#
TweetinviEvents.UnsubscribeFromClientEvents(client);
```
</div>