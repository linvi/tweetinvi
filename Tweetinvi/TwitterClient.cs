using System;
using Tweetinvi.Client;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;

// ReSharper disable once CheckNamespace
namespace Tweetinvi
{
    public class TwitterClientParameters
    {
        public TwitterClientParameters()
        {
            Settings = new TweetinviSettings();
        }

        public IRateLimitCache RateLimitCache { get; set; }
        public ITweetinviContainer Container { get; set; }
        public ITweetinviSettings Settings { get; set; }
        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;

        public void RaiseBeforeRegistrationCompletes(TweetinviContainerEventArgs args)
        {
            args.TweetinviContainer.Raise(BeforeRegistrationCompletes, args);
        }
    }

    public class TwitterClient : ITwitterClient, IDisposable
    {
        private IReadOnlyTwitterCredentials _credentials;
        private readonly ITweetinviContainer _tweetinviContainer;

        private readonly ITwitterClientEvents _twitterClientEvents;
        private readonly ITweetinviEvents _tweetinviEvents;

        /// <summary>
        /// IMPORTANT NOTE: The setter is for convenience. It is strongly recommended to create a new TwitterClient instead.
        /// As using this setter could result in unexpected concurrency between the time of set and the execution of previous
        /// non awaited async operations.
        /// </summary>
        public IReadOnlyTwitterCredentials Credentials
        {
            get => _credentials;
            set => _credentials = new ReadOnlyTwitterCredentials(value);
        }

        public ITweetinviSettings ClientSettings { get; }

        public TwitterClient(IReadOnlyTwitterCredentials credentials) : this(credentials, new TwitterClientParameters())
        {
        }

        public TwitterClient(IReadOnlyTwitterCredentials credentials, TwitterClientParameters parameters)
        {
            Credentials = credentials;
            ClientSettings = parameters?.Settings ?? new TweetinviSettings();

            if (parameters?.Container == null)
            {
                if (!TweetinviContainer.Container.IsInitialized)
                {
                    TweetinviContainer.Container.Initialize();
                }
            }
            else
            {
                if (!parameters.Container.IsInitialized)
                {
                    throw new InvalidOperationException("Cannot create a client with a non initialized container!");
                }
            }

            _tweetinviContainer = new Injectinvi.TweetinviContainer(parameters?.Container ?? TweetinviContainer.Container);

            if (parameters?.RateLimitCache != null)
            {
                _tweetinviContainer.RegisterInstance(typeof(IRateLimitCache), parameters.RateLimitCache);
            }

            void BeforeRegistrationDelegate(object sender, TweetinviContainerEventArgs args)
            {
                parameters?.RaiseBeforeRegistrationCompletes(args);
            };

            _tweetinviContainer.BeforeRegistrationCompletes += BeforeRegistrationDelegate;
            _tweetinviContainer.Initialize();
            _tweetinviContainer.BeforeRegistrationCompletes -= BeforeRegistrationDelegate;

            _tweetinviEvents = _tweetinviContainer.Resolve<ITweetinviEvents>();

            var requestExecutor = _tweetinviContainer.Resolve<IInternalRequestExecutor>();
            requestExecutor.Initialize(this);
            RequestExecutor = requestExecutor;

            var parametersValidator = _tweetinviContainer.Resolve<IInternalParametersValidator>();
            parametersValidator.Initialize(this);
            ParametersValidator = parametersValidator;

            Account = new AccountClient(this);
            Auth = new AuthClient(this);
            AccountSettings = new AccountSettingsClient(this);
            RateLimits = new RateLimitsClient(this);
            Timeline = new TimelineClient(this);
            Tweets = new TweetsClient(this);
            Upload = new UploadClient(this);
            Users = new UsersClient(this);

            _twitterClientEvents = _tweetinviContainer.Resolve<ITwitterClientEvents>();
            Events.BeforeWaitingForRequestRateLimits += EventsOnBeforeWaitingForRequestRateLimits;
            Events.BeforeExecutingRequest += EventsOnBeforeExecutingRequest;
            Events.AfterExecutingRequest += EventsOnAfterExecutingRequest;
            Events.OnTwitterException += EventsOnOnTwitterException;

            var rateLimitCacheManager = _tweetinviContainer.Resolve<IRateLimitCacheManager>();
            rateLimitCacheManager.RateLimitsClient = RateLimits;
        }

        public IAccountClient Account { get; }
        public IAuthClient Auth { get; }
        public IAccountSettingsClient AccountSettings { get; }
        public IRateLimitsClient RateLimits { get; }
        public ITimelineClient Timeline { get; }
        public ITweetsClient Tweets { get; }
        public IUploadClient Upload { get; }
        public IUsersClient Users { get; }
        public IExternalClientEvents Events => _twitterClientEvents;

        public IParametersValidator ParametersValidator { get; }
        public IRequestExecutor RequestExecutor { get; }

        public ITwitterExecutionContext CreateTwitterExecutionContext()
        {
            return new TwitterExecutionContext
            {
                RequestFactory = CreateRequest,
                Container = _tweetinviContainer,
                Events = _twitterClientEvents
            };
        }

        public ITwitterRequest CreateRequest()
        {
            var request = new TwitterRequest
            {
                ExecutionContext = CreateTwitterExecutionContext(),
                Query =
                {
                    // we are cloning here to ensure that the context will never be modified regardless of concurrency
                    TwitterCredentials = new TwitterCredentials(Credentials)
                }
            };

            request.ExecutionContext.InitialiseFrom(ClientSettings);

            return request;
        }

        private void EventsOnBeforeWaitingForRequestRateLimits(object sender, BeforeExecutingRequestEventArgs e)
        {
            _tweetinviEvents.RaiseBeforeWaitingForQueryRateLimits(e);
        }

        private void EventsOnBeforeExecutingRequest(object sender, BeforeExecutingRequestEventArgs e)
        {
            _tweetinviEvents.RaiseBeforeExecutingQuery(e);
        }

        private void EventsOnAfterExecutingRequest(object sender, AfterExecutingQueryEventArgs e)
        {
            _tweetinviEvents.RaiseAfterExecutingQuery(e);
        }

        private void EventsOnOnTwitterException(object sender, ITwitterException e)
        {
            _tweetinviEvents.RaiseOnTwitterException(e);
        }

        public void Dispose()
        {
            Events.BeforeWaitingForRequestRateLimits -= EventsOnBeforeWaitingForRequestRateLimits;
            Events.BeforeExecutingRequest -= EventsOnBeforeExecutingRequest;
            Events.AfterExecutingRequest -= EventsOnAfterExecutingRequest;
            Events.OnTwitterException -= EventsOnOnTwitterException;
        }
    }
}
