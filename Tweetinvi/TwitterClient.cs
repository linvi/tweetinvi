using System;
using System.Collections.Immutable;
using Tweetinvi.Client;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Injectinvi;
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
    }

    public class TwitterClient : ITwitterClient
    {
        private IReadOnlyTwitterCredentials _credentials;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly ITweetinviContainer _tweetinviContainer;

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

            _tweetinviContainer.Initialize();

            var requestExecutor = _tweetinviContainer.Resolve<IInternalRequestExecutor>();
            requestExecutor.Initialize(this);
            RequestExecutor = requestExecutor;

            var parametersValidator = _tweetinviContainer.Resolve<IInternalParametersValidator>();
            parametersValidator.Initialize(this);
            ParametersValidator = parametersValidator;

            _rateLimitCacheManager = _tweetinviContainer.Resolve<IRateLimitCacheManager>();

            Account = new AccountClient(this);
            Auth = new AuthClient(this);
            AccountSettings = new AccountSettingsClient(this);
            RateLimits = new RateLimitsClient(this, _rateLimitCacheManager);
            Timeline = new TimelineClient(this);
            Tweets = new TweetsClient(this);
            Upload = new UploadClient(this);
            Users = new UsersClient(this);

            _rateLimitCacheManager.RateLimitsClient = RateLimits;
        }

        public IAccountClient Account { get; }
        public IAuthClient Auth { get; }
        public IAccountSettingsClient AccountSettings { get; }
        public IRateLimitsClient RateLimits { get; }
        public ITimelineClient Timeline { get; }
        public ITweetsClient Tweets { get; }
        public IUploadClient Upload { get; }
        public IUsersClient Users { get; }

        public IParametersValidator ParametersValidator { get; }
        public IRequestExecutor RequestExecutor { get; }

        public ITwitterExecutionContext CreateTwitterExecutionContext()
        {
            return new TwitterExecutionContext
            {
                RequestFactory = CreateRequest,
                RateLimitCacheManager = _rateLimitCacheManager,
                Container = _tweetinviContainer
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
    }
}
