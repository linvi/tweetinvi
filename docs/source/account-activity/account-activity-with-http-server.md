# Account Activity with HttpServer

During the registration process of a webhook url, Twitter will send an http request to your webhook url.\
This request is called a CRC request. Twitter expect your application to send a response containing a special authentication token.

Tweetinvi offer a `RequestHandler` that checks all incoming http requests to your server.\
When the `RequestHandler` identifies that a request should be handled by Tweetinvi (like the CRC request), tweetinvi will take care of the response for you.

<details>
<summary>SimpleHttpServer.cs</summary>

``` c#
using System;
using System.Net;
using System.Threading.Tasks;

namespace tweetinvi_hello_world
{
    public class SimpleHttpServer : IDisposable
    {
        private readonly HttpListener _server;

        public SimpleHttpServer(int port)
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://*:" + port + "/");
        }

        public EventHandler<HttpListenerContext> OnRequest;

        public void Start()
        {
            _server.Start();
            RunServerAsync(); // do not await
        }

        private async Task RunServerAsync()
        {
            while (_server.IsListening)
            {
                var context = await _server.GetContextAsync();
                OnRequest?.Invoke(this, context);
            }
        }

        public async Task WaitUntilDisposed()
        {
            while (!_disposed)
            {
                await Task.Delay(200);
            }
        }

        private bool _disposed;
        public void Dispose()
        {
            _disposed = true;
            ((IDisposable) _server)?.Dispose();
        }
    }
}
```

</details>

Here is a sample server capable of handling twitter requests.

``` c#
Plugins.Add<AspNetPlugin>();

var appClient = new TwitterClient(appCredsWithBearerToken);
var userClient = new TwitterClient(userCreds);

var accountActivityHandler = appClient.AccountActivity.CreateRequestHandler();

var httpServer = new SimpleHttpServer(8042);
httpServer.OnRequest += async (sender, context) =>
{
    var webhookRequest = WebhookRequestFactory.Create(context);

    // check if the request comes from twitter
    if (await accountActivityHandler.IsRequestManagedByTweetinviAsync(webhookRequest))
    {
        // let tweetinvi manage the twitter requests and respond to it
        await accountActivityHandler.TryRouteRequestAsync(webhookRequest);
    }
    else
    {
        // a request not coming from twitter (probably for a user of your website)
        var streamWriter = new StreamWriter(context.Response.OutputStream);
        await streamWriter.WriteAsync("Hello friend, this page does not exists!");
        await streamWriter.FlushAsync();
        context.Response.StatusCode = 404;
        context.Response.Close();
    }
};

httpServer.Start();

await httpServer.WaitUntilDisposed();
```

