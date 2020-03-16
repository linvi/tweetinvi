using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Webhooks;
using Tweetinvi.Streams;

namespace Tweetinvi.Core.Logic
{
    public class AccountActivityRequestHandler : IAccountActivityRequestHandler
    {
        private readonly IWebhookDispatcher _dispatcher;
        private readonly IFactory<AccountActivityStream> _accountActivityStreamFactory;
        private readonly IWebhookRouter _router;
        private readonly IConsumerOnlyCredentials _consumerOnlyCredentials;

        public AccountActivityRequestHandler(
            IWebhookDispatcher dispatcher,
            IWebhooksRoutes routes,
            IWebhooksHelper webhooksHelper,
            IFactory<AccountActivityStream> accountActivityStreamFactory,
            ITwitterClient client)
        {
            _dispatcher = dispatcher;
            _accountActivityStreamFactory = accountActivityStreamFactory;
            _router = new WebhookRouter(dispatcher, routes, webhooksHelper);

            _consumerOnlyCredentials = new ConsumerOnlyCredentials(client.Credentials);
        }

        public Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request)
        {
            return _router.IsRequestManagedByTweetinvi(request);
        }

        public async Task<bool> TryRouteRequest(IWebhooksRequest request)
        {
            return await _router.TryRouteRequest(request, _consumerOnlyCredentials).ConfigureAwait(false);
        }

        private readonly Dictionary<long, IAccountActivityStream> _accountActivityStreams = new Dictionary<long, IAccountActivityStream>();
        public IAccountActivityStream GetAccountActivityStream(long userId, string environment)
        {
            if (_accountActivityStreams.ContainsKey(userId))
            {
                return _accountActivityStreams[userId];
            }

            var stream = _accountActivityStreamFactory.Create();
            stream.AccountUserId = userId;

            _accountActivityStreams.Add(userId, stream);
            _dispatcher.SubscribeAccountActivityStream(stream);

            return stream;
        }
    }
}