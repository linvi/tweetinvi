using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Factories.Lists;
using Tweetinvi.Factories.SavedSearch;
using Tweetinvi.Factories.Tweet;
using Tweetinvi.Factories.User;
using Tweetinvi.Models;

namespace Tweetinvi.Factories
{
    public class TweetinviFactoriesModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<ITweetFactory, TweetFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IUserFactory, UserFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IMessageFactory, MessageFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IMessageFactoryQueryExecutor, MessageFactoryQueryExecutor>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITwitterListIdentifierFactory, TwitterListIdentifierFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ISavedSearchFactory, SavedSearchFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchJsonFactory, SavedSearchJsonFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchQueryExecutor, SavedSearchFactoryQueryExecutor>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ISavedSearchQueryGenerator, SavedSearchFactoryQueryGenerator>(RegistrationLifetime.InstancePerApplication);

            // This is instance per thread as we have a CredentialsAccessor that is an instance per thread.
            container.RegisterType<ITwitterQueryFactory, TwitterQueryFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITwitterRequestFactory, TwitterRequestFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterResultFactory, TwitterResultFactory>(RegistrationLifetime.InstancePerApplication);
        }
    }
}