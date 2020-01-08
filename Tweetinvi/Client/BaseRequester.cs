using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public interface IBaseRequester
    {
        void Initialize(ITwitterClient client);
    }

    public abstract class BaseRequester : IBaseRequester
    {
        protected ITwitterClient TwitterClient { get; private set; }
        private ITwitterClientEvents _twitterClientEvents;

        public void Initialize(ITwitterClient client)
        {
            if (TwitterClient != null)
            {
                throw new InvalidOperationException("createRequest cannot be changed");
            }

            TwitterClient = client;

            _twitterClientEvents = client.CreateTwitterExecutionContext().Container.Resolve<ITwitterClientEvents>();
        }

        public ITwitterRequest CreateRequest()
        {
            return TwitterClient.CreateRequest();
        }

        protected async Task ExecuteRequest(Func<Task> action, ITwitterRequest request)
        {
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (TwitterException ex)
            {
                _twitterClientEvents.RaiseOnTwitterException(ex);
                throw;
            }
        }

        protected async Task ExecuteRequest(Func<ITwitterRequest, Task> action)
        {
            try
            {
                var request = TwitterClient.CreateRequest();
                await action(request).ConfigureAwait(false);
            }
            catch (TwitterException ex)
            {
                _twitterClientEvents.RaiseOnTwitterException(ex);
                throw;
            }
        }

        protected async Task<T> ExecuteRequest<T>(Func<ITwitterRequest, Task<T>> action) where T : class
        {
            try
            {
                var request = TwitterClient.CreateRequest();
                return await action(request).ConfigureAwait(false);
            }
            catch (TwitterException ex)
            {
                _twitterClientEvents.RaiseOnTwitterException(ex);
                throw;
            }
        }

        protected async Task<T> ExecuteRequest<T>(Func<Task<T>> action, ITwitterRequest request) where T : class
        {
            try
            {
                return await action().ConfigureAwait(false);
            }
            catch (TwitterException ex)
            {
                _twitterClientEvents.RaiseOnTwitterException(ex);
                throw;
            }
        }
    }
}
