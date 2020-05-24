# Account Activity - AspNet

During the registration process of a webhook url, Twitter will send an http request to your webhook url.\
This request is called a CRC request. Twitter expect your application to send a response containing a special authentication token.

Tweetinvi offer a `RequestHandler` that checks all incoming http requests to your server.\
When the `RequestHandler` identifies that a request should be handled by Tweetinvi (like the CRC request), tweetinvi will take care of the response for you.

In addition the `RequestHandler` will filter and route the twitter requests containing user events.

``` c#
// I suggest you use a client that contains both a bearer token and an access token
var credentials = new TwitterCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET")
{
    BearerToken = "BEARER_TOKEN"
};

var requestHandler = webhookClient.AccountActivity.CreateRequestHandler();
var config = new WebhookMiddlewareConfiguration(AccountActivityRequestHandler);

// Use Tweetinvi AspNet middleware
app.UseTweetinviWebhooks(config);
```

## Example

``` c#
// Startup.cs
public class Startup
{
    public static IAccountActivityRequestHandler AccountActivityRequestHandler { get; set; }
    public static ITwitterClient WebhookClient { get; set; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // webhook initialization
        Plugins.Add<AspNetPlugin>();

        var credentials = new TwitterCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET")
        {
            BearerToken = "BEARER_TOKEN"
        };

        WebhookClient = new TwitterClient(credentials);
        AccountActivityRequestHandler = WebhookClient.AccountActivity.CreateRequestHandler();
        var config = new WebhookMiddlewareConfiguration(AccountActivityRequestHandler);

        app.UseTweetinviWebhooks(config);

        // aspnet initialization
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
```