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
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IWebRequestExecutor, WebRequestExecutor>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITwitterRequestHandler, TwitterRequestHandler>();

            container.RegisterType<IConsumerCredentials, ConsumerCredentials>();
            container.RegisterType<ITwitterCredentials, TwitterCredentials>();

            container.RegisterType<IUploadParameters, UploadParameters>();
            container.RegisterType<IUploadOptionalParameters, UploadOptionalParameters>();
            container.RegisterType<IUploadVideoParameters, UploadVideoParameters>();
            container.RegisterType<IUploadVideoOptionalParameters, UploadVideoOptionalParameters>();

            container.RegisterType<IOAuthQueryParameter, OAuthQueryParameter>();
            container.RegisterType<IOAuthWebRequestGenerator, OAuthWebRequestGenerator>();

            container.RegisterType<IWebHelper, WebHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IHttpClientWebHelper, HttpClientWebHelper>();
            container.RegisterType<IWebRequestResult, WebRequestResult>();

            container.RegisterType<ITwitterQuery, TwitterQuery>();
        }
    }
}