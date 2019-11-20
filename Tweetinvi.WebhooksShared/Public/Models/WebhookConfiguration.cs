using System;
using System.Collections.Generic;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Webhooks;

namespace Tweetinvi.Models
{
    public interface IWebhookConfiguration
    {
        IConsumerOnlyCredentials ConsumerOnlyCredentials { get; }

        IRegistrableWebhookEnvironment[] RegisteredWebhookEnvironments { get; }
        IAccountActivityStream[] RegisteredActivityStreams { get; }
        IWebhookDispatcher WebhookDispatcher { get; }

        void AddWebhookEnvironment(IRegistrableWebhookEnvironment environment);
        void RemoveWebhookEnvironment(IRegistrableWebhookEnvironment environment);
        void AddActivityStream(IAccountActivityStream accountActivityStream);
        void RemoveActivityStream(IAccountActivityStream accountActivityStream);
    }

    public class WebhookConfiguration : IWebhookConfiguration
    {
        private readonly List<IRegistrableWebhookEnvironment> _webhookEnvironments;

        public WebhookConfiguration()
        {
            _webhookEnvironments = new List<IRegistrableWebhookEnvironment>();
            WebhookDispatcher = TweetinviContainer.Resolve<IWebhookDispatcher>();
        }

        public WebhookConfiguration(IConsumerOnlyCredentials consumerOnlyCredentials) : this()
        {
            ConsumerOnlyCredentials = consumerOnlyCredentials;
        }

        public IWebhookDispatcher WebhookDispatcher { get; }

        public IConsumerOnlyCredentials ConsumerOnlyCredentials { get; }

        public IRegistrableWebhookEnvironment[] RegisteredWebhookEnvironments => _webhookEnvironments.ToArray();

        public IAccountActivityStream[] RegisteredActivityStreams => WebhookDispatcher.SubscribedAccountActivityStreams;

        public void AddWebhookEnvironment(IRegistrableWebhookEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (environment.Credentials != ConsumerOnlyCredentials)
            {
                throw new InvalidOperationException("Tweetinvi only supports 1 set of consumer application be webhooks server instance.");
            }

            if (_webhookEnvironments.Contains(environment))
            {
                return;
            }

            _webhookEnvironments.Add(environment);
        }

        public void RemoveWebhookEnvironment(IRegistrableWebhookEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _webhookEnvironments.Remove(environment);
        }

        public void AddActivityStream(IAccountActivityStream accountActivityStream)
        {
            WebhookDispatcher.SubscribeAccountActivityStream(accountActivityStream);
        }

        public void RemoveActivityStream(IAccountActivityStream accountActivityStream)
        {
            WebhookDispatcher.UnsubscribeAccountActivityStream(accountActivityStream);
        }
    }
}
