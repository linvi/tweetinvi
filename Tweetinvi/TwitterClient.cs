using Tweetinvi.Client;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.Models;

// ReSharper disable once CheckNamespace
namespace Tweetinvi
{
    public class TwitterClientParameters
    {
        public IRateLimitCache RateLimitCache { get; set; }
    }

    public class TwitterClient : ITwitterClient
    {
        private IReadOnlyTwitterCredentials _credentials;
        private IRateLimitCacheManager _rateLimitCacheManager;

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

        public ITweetinviSettings Config { get; }

        public TwitterClient(IReadOnlyTwitterCredentials credentials) : this(credentials, new TwitterClientParameters())
        {
        }

        public TwitterClient(IReadOnlyTwitterCredentials credentials, TwitterClientParameters parameters)
        {
            Credentials = credentials;
            Config = new TweetinviSettings();

            var requestExecutor = TweetinviContainer.Resolve<IInternalRequestExecutor>();
            requestExecutor.Initialize(this);
            RequestExecutor = requestExecutor;

            var parametersValidator = TweetinviContainer.Resolve<IInternalParametersValidator>();
            parametersValidator.Initialize(this);
            ParametersValidator = parametersValidator;

            _rateLimitCacheManager = TweetinviContainer.Resolve<IRateLimitCacheManager>();
            if (parameters?.RateLimitCache != null)
            {
                _rateLimitCacheManager.RateLimitCache = parameters.RateLimitCache;
            }

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
                RequestFactory = CreateRequest
            };
        }

        public ITwitterRequest CreateRequest()
        {
            var request = new TwitterRequest
            {
                ExecutionContext = new TwitterExecutionContext
                {
                    RequestFactory = CreateRequest,
                    RateLimitCacheManager = _rateLimitCacheManager
                },
                Query =
                {
                    // we are cloning here to ensure that the context will never be modified regardless of concurrency
                    TwitterCredentials = new TwitterCredentials(Credentials)
                }
            };

            request.ExecutionContext.InitialiseFrom(Config);

            return request;
        }
    }
}
