using Tweetinvi.Client;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

// ReSharper disable once CheckNamespace
namespace Tweetinvi
{
    public class TwitterClient : ITwitterClient
    {
        public ITwitterCredentials Credentials { get; }
        public ITweetinviSettings Config { get; }

        public TwitterClient(ITwitterCredentials credentials)
        {
            Credentials = credentials;
            Config = new TweetinviSettings();

            var requestExecutor = TweetinviContainer.Resolve<IInternalRequestExecutor>();
            requestExecutor.Initialize(this);
            RequestExecutor = requestExecutor;

            var parametersValidator = TweetinviContainer.Resolve<IInternalParametersValidator>();
            parametersValidator.Initialize(this);
            ParametersValidator = parametersValidator;

            Account = new AccountClient(this);
            AccountSettings = new AccountSettingsClient(this);
            Timeline = new TimelineClient(this);
            Tweets = new TweetsClient(this);
            Upload = new UploadClient(this);
            Users = new UsersClient(this);
        }

        public IAccountClient Account { get; }
        public IAccountSettingsClient AccountSettings { get; }
        public ITimelineClient Timeline { get; set; }
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
                    RequestFactory = CreateRequest
                },
                Query =
                {
                    TwitterCredentials = Credentials
                }
            };

            request.ExecutionContext.InitialiseFrom(Config);

            return request;
        }
    }
}
