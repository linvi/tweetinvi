# Tweetinvi Container

**IMPORTANT** : this page is for very advance use cases and is not recommended if you are not sure of what you are doing.

Tweetinvi offer developers ways to change any logic in its flow by overriding a component registration.\
When you do so, the newly registered component will be used instead of the default Tweetinvi implementation.

## Example

Lets assume you wanted to change the implementation of the `TwitterRequestHandler` so that you can add your own logs.

``` c#
class MyRequestHandler : ITwitterRequestHandler
{
    private ITwitterRequestHandler _tweetinviRequestHandler;

    public MyRequestHandler()
    {
        _tweetinviRequestHandler = TweetinviContainer.Resolve<ITwitterRequestHandler>();
    }

    public Task<ITwitterResponse> ExecuteQueryAsync(ITwitterRequest request)
    {
        // My custom logic :)
        System.Console.WriteLine(request.Query.Url);
        return _tweetinviRequestHandler.ExecuteQueryAsync(request);
    }
}

// 1. Create a custom container
var container = new Tweetinvi.Injectinvi.TweetinviContainer();
container.BeforeRegistrationCompletes += (sender, args) =>
{
    // 2. Register your own code logic
    container.RegisterInstance(typeof(ITwitterRequestHandler), new MyRequestHandler());
};
// 3. Initialise the container
container.Initialize();

// 4. Pass the container to the client
var userClient = new TwitterClient(creds, new TwitterClientParameters()
{
    Container = container
});

// Any request executed by the userClient will now use `MyRequestHandler` implementation
```