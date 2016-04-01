using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Core.Web;

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
            _container.RegisterType<ITwitterRequester, TwitterRequester>();
            _container.RegisterType<ITwitterRequestGenerator, TwitterRequestGenerator>();

            _container.RegisterType<IConsumerCredentials, ConsumerCredentials>();
            _container.RegisterType<ITwitterCredentials, TwitterCredentials>();

            _container.RegisterType<IUploadQueryParameters, UploadQueryParameters>();

            _container.RegisterType<IOAuthQueryParameter, OAuthQueryParameter>();
            _container.RegisterType<IOAuthWebRequestGenerator, OAuthWebRequestGenerator>();

            _container.RegisterType<IWebHelper, WebHelper>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<IHttpClientWebHelper, HttpClientWebHelper>();
            _container.RegisterType<IWebRequestResult, WebRequestResult>();
            _container.RegisterType<IWebProxyFactory, WebProxyFactory>(RegistrationLifetime.InstancePerApplication);
        }
    }
}