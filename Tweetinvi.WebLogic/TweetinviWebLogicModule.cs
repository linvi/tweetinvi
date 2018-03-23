using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.WebLogic
{
    public class TweetinviWebLogicModule : ITweetinviModule
    {
        private readonly ITweetinviContainer _container;

        public TweetinviWebLogicModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IWebRequestExecutor, WebRequestExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterRequestHandler, TwitterRequestHandler>();

            _container.RegisterType<IConsumerCredentials, ConsumerCredentials>();
            _container.RegisterType<ITwitterCredentials, TwitterCredentials>();

            _container.RegisterType<IUploadParameters, UploadParameters>();
            _container.RegisterType<IUploadOptionalParameters, UploadOptionalParameters>();
            _container.RegisterType<IUploadVideoParameters, UploadVideoParameters>();
            _container.RegisterType<IUploadVideoOptionalParameters, UploadVideoOptionalParameters>();

            _container.RegisterType<IOAuthQueryParameter, OAuthQueryParameter>();
            _container.RegisterType<IOAuthWebRequestGenerator, OAuthWebRequestGenerator>();

            _container.RegisterType<IWebHelper, WebHelper>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IHttpClientWebHelper, HttpClientWebHelper>();
            _container.RegisterType<IWebRequestResult, WebRequestResult>();

            _container.RegisterType<ITwitterQuery, TwitterQuery>();
        }
    }
}