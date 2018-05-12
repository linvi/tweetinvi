using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Factories.Friendship;
using Tweetinvi.Factories.Geo;
using Tweetinvi.Factories.Lists;
using Tweetinvi.Factories.SavedSearch;
using Tweetinvi.Factories.Search;
using Tweetinvi.Factories.Tweet;
using Tweetinvi.Factories.User;
using Tweetinvi.Models;

namespace Tweetinvi.Factories
{
    public class TweetinviFactoriesModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<ISearchResultFactory, SearchResultFactory>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<ITweetFactory, TweetFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITweetFactoryQueryExecutor, TweetFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<IUserFactory, UserFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IUserFactoryQueryExecutor, UserFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<IFriendshipFactory, FriendshipFactory>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<IMessageFactory, MessageFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<IMessageFactoryQueryExecutor, MessageFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<ITwitterListIdentifierFactory, TwitterListIdentifierFactory>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITwitterListFactory, TwitterListFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITwitterListFactoryQueryExecutor, TwitterListFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ITwitterListFactoryQueryGenerator, TwitterListFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<IGeoFactory, GeoFactory>(RegistrationLifetime.InstancePerThread);

            container.RegisterType<ISavedSearchFactory, SavedSearchFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ISavedSearchJsonFactory, SavedSearchJsonFactory>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ISavedSearchQueryExecutor, SavedSearchFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            container.RegisterType<ISavedSearchQueryGenerator, SavedSearchFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);

            // This is instance per thread as we have a CredentialsAccessor that is an instance per thread.
            container.RegisterType<ITwitterQueryFactory, TwitterQueryFactory>(RegistrationLifetime.InstancePerThread);
        }
    }
}