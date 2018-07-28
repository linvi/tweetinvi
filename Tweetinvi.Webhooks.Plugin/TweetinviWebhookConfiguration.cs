using System;
using System.Collections.Generic;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Models;
using Tweetinvi.Webhooks.Plugin.Models;

namespace Tweetinvi.WebLogic.Webhooks
{
    public interface ITweetinviWebhookConfiguration
    {
        IRegistrableWebhookEnvironment[] RegisteredWebhookEnvironments { get; }
        IAccountActivityStream[] RegisteredActivityStreams { get; }

        void AddWebhookEnvironment(IRegistrableWebhookEnvironment environment);
        void RemoveWebhookEnvironment(IRegistrableWebhookEnvironment environment);
        void AddActivityStream(IAccountActivityStream accountActivityStream);
        void RemoveActivityStream(IAccountActivityStream accountActivityStream);
    }

    public class TweetinviWebhookConfiguration : ITweetinviWebhookConfiguration
    {
        private readonly IConsumerCredentials _consumerCredentials;
        private List<IAccountActivityStream> _userAccountActivityStreams;
        private List<IRegistrableWebhookEnvironment> _webhookEnvironments;

        public TweetinviWebhookConfiguration()
        {
            _userAccountActivityStreams = new List<IAccountActivityStream>();
            _webhookEnvironments = new List<IRegistrableWebhookEnvironment>();
        }

        public TweetinviWebhookConfiguration(IConsumerCredentials consumerCredentials) : this()
        {
            _consumerCredentials = consumerCredentials;
        }

        public IRegistrableWebhookEnvironment[] RegisteredWebhookEnvironments
        {
            get { return _webhookEnvironments.ToArray(); }
        }

        public IAccountActivityStream[] RegisteredActivityStreams
        {
            get { return _userAccountActivityStreams.ToArray(); }
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
            if (accountActivityStream == null)
            {
                throw new ArgumentNullException(nameof(accountActivityStream));
            }

            if (_userAccountActivityStreams.Contains(accountActivityStream))
            {
                return;
            }

            _userAccountActivityStreams.Add(accountActivityStream);
        }

        public void RemoveActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (accountActivityStream == null)
            {
                throw new ArgumentNullException(nameof(accountActivityStream));
            }

            _userAccountActivityStreams.Remove(accountActivityStream);
        }
    }
}
