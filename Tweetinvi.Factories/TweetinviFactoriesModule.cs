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
        private readonly ITweetinviContainer _container;

        public TweetinviFactoriesModule(ITweetinviContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ISearchResultFactory, SearchResultFactory>(RegistrationLifetime.InstancePerApplication);

            _container.RegisterType<ITweetFactory, TweetFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITweetFactoryQueryExecutor, TweetFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IUserFactory, UserFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IUserFactoryQueryExecutor, UserFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IFriendshipFactory, FriendshipFactory>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IMessageFactory, MessageFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<IMessageFactoryQueryExecutor, MessageFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ITwitterListIdentifierFactory, TwitterListIdentifierFactory>(RegistrationLifetime.InstancePerApplication);
            _container.RegisterType<ITwitterListFactory, TwitterListFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterListFactoryQueryExecutor, TwitterListFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ITwitterListFactoryQueryGenerator, TwitterListFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<IGeoFactory, GeoFactory>(RegistrationLifetime.InstancePerThread);

            _container.RegisterType<ISavedSearchFactory, SavedSearchFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchJsonFactory, SavedSearchJsonFactory>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchQueryExecutor, SavedSearchFactoryQueryExecutor>(RegistrationLifetime.InstancePerThread);
            _container.RegisterType<ISavedSearchQueryGenerator, SavedSearchFactoryQueryGenerator>(RegistrationLifetime.InstancePerThread);

            // This is instance per thread as we have a CredentialsAccessor that is an instance per thread.
            _container.RegisterType<ITwitterQueryFactory, TwitterQueryFactory>(RegistrationLifetime.InstancePerThread);
        }
    }
}