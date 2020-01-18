using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Lists
{
    public class TwitterListFactory : ITwitterListFactory
    {
        private readonly ITwitterListFactoryQueryExecutor _twitterListFactoryQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;

        public TwitterListFactory(
            ITwitterListFactoryQueryExecutor twitterListFactoryQueryExecutor,
            ITwitterClientFactories factories,
            ITwitterListIdentifierFactory twitterListIdentifierFactory)
        {
            _twitterListFactoryQueryExecutor = twitterListFactoryQueryExecutor;
            _factories = factories;
            _twitterListIdentifierFactory = twitterListIdentifierFactory;
        }

        // Get Existing
        public Task<ITwitterList> GetExistingList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetExistingList(identifier);
        }

        public Task<ITwitterList> GetExistingList(string slug, IUserIdentifier user)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, user);
            return GetExistingList(identifier);
        }

        public Task<ITwitterList> GetExistingList(string slug, long userId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, userId);
            return GetExistingList(identifier);
        }

        public Task<ITwitterList> GetExistingList(string slug, string userScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, userScreenName);
            return GetExistingList(identifier);
        }

        public async Task<ITwitterList> GetExistingList(ITwitterListIdentifier identifier)
        {
            if (identifier == null)
            {
                return null;
            }

            var listDTO = await _twitterListFactoryQueryExecutor.GetExistingList(identifier);
            return _factories.CreateTwitterList(listDTO);
        }

        // Generate List from DTO

        public IEnumerable<ITwitterList> CreateListsFromDTOs(IEnumerable<ITwitterListDTO> listDTOs)
        {
            if (listDTOs == null)
            {
                return null;
            }

            return listDTOs.Select(_factories.CreateTwitterList).ToArray();
        }

    }
}