using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.WebLogic
{
    public class TweetinviWebLogicModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IWebRequestExecutor, WebRequestExecutor>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterRequestHandler, TwitterRequestHandler>();

            container.RegisterType<IConsumerCredentials, ConsumerCredentials>();
            container.RegisterType<ITwitterCredentials, TwitterCredentials>();

            container.RegisterType<IOAuthQueryParameter, OAuthQueryParameter>();
            container.RegisterType<IOAuthWebRequestGeneratorFactory, OAuthWebRequestGeneratorFactory>();

            container.RegisterType<IWebHelper, WebHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHttpClientWebHelper, HttpClientWebHelper>();
            container.RegisterType<ITwitterResponse, TwitterResponse>();

            container.RegisterType<ITwitterQuery, TwitterQuery>();
        }
    }
}