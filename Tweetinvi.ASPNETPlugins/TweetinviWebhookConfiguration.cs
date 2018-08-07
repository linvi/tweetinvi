using System;
using System.Collections.Generic;
using Tweetinvi.ASPNETPlugins.Models;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models;

namespace Tweetinvi.ASPNETPlugins
{
    public interface ITweetinviWebhookConfiguration
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

    public class TweetinviWebhookConfiguration : ITweetinviWebhookConfiguration
    {
        private List<IRegistrableWebhookEnvironment> _webhookEnvironments;

        public TweetinviWebhookConfiguration()
        {
            _webhookEnvironments = new List<IRegistrableWebhookEnvironment>();
            WebhookDispatcher = TweetinviContainer.Resolve<IWebhookDispatcher>();
        }

        public TweetinviWebhookConfiguration(IConsumerOnlyCredentials consumerOnlyCredentials) : this()
        {
            ConsumerOnlyCredentials = consumerOnlyCredentials;
        }

        public IWebhookDispatcher WebhookDispatcher { get; }

        public IConsumerOnlyCredentials ConsumerOnlyCredentials { get; }

        public IRegistrableWebhookEnvironment[] RegisteredWebhookEnvironments
        {
            get { return _webhookEnvironments.ToArray(); }
        }

        public IAccountActivityStream[] RegisteredActivityStreams
        {
            get { return WebhookDispatcher.SubscribedAccountActivityStreams; }
        }

        public void AddWebhookEnvironment(IRegistrableWebhookEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
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
