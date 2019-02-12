using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public static class WebhookMiddlewareExtensions
    {
        public static void UseTweetinviWebhooks(this Collection<DelegatingHandler> messageHandlers, IWebhookConfiguration configuration)
        {
            messageHandlers.Add(new WebhookMiddlewareMessageHandler(configuration));
        }
    }

    public class WebhookMiddlewareMessageHandler : DelegatingHandler
    {
        private readonly IWebhookConfiguration _configuration;
        private readonly IWebhookRouter _router;

        public WebhookMiddlewareMessageHandler(IWebhookConfiguration configuration)
        {
            _configuration = configuration;
            _router = WebhooksPlugin.Container.Resolve<IWebhookRouter>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestHandler = new WebhooksRequestHandlerForWebApi(request);

            if (_router.IsRequestManagedByTweetinvi(requestHandler, _configuration))
            {
                var routeHandled = await _router.TryRouteRequest(requestHandler, _configuration);

                if (routeHandled)
                {
                    var response = requestHandler.GetHttpResponseMessage();

                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);

                    return await tsc.Task;
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
