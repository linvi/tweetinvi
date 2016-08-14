using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Lists
{
    public class TwitterListFactory : ITwitterListFactory
    {
        private readonly ITwitterListFactoryQueryExecutor _twitterListFactoryQueryExecutor;
        private readonly IFactory<ITwitterList> _twitterListFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;

        public TwitterListFactory(
            ITwitterListFactoryQueryExecutor twitterListFactoryQueryExecutor,
            IFactory<ITwitterList> twitterListFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITwitterListIdentifierFactory twitterListIdentifierFactory)
        {
            _twitterListFactoryQueryExecutor = twitterListFactoryQueryExecutor;
            _twitterListFactory = twitterListFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _twitterListIdentifierFactory = twitterListIdentifierFactory;
        }

        // Create List
        public ITwitterList CreateList(string name, PrivacyMode privacyMode, string description)
        {
            var listDTO = _twitterListFactoryQueryExecutor.CreateList(name, privacyMode, description);
            return CreateListFromDTO(listDTO);
        }

        // Get Existing
        public ITwitterList GetExistingList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetExistingList(identifier);
        }

        public ITwitterList GetExistingList(string slug, IUser user)
        {
            if (user == null)
            {
                return null;
            }

            return GetExistingList(slug, user.UserDTO);
        }

        public ITwitterList GetExistingList(string slug, IUserIdentifier userIdentifier)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, userIdentifier);
            return GetExistingList(identifier);
        }

        public ITwitterList GetExistingList(string slug, long userId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, userId);
            return GetExistingList(identifier);
        }

        public ITwitterList GetExistingList(string slug, string userScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, userScreenName);
            return GetExistingList(identifier);
        }

        public ITwitterList GetExistingList(ITwitterListIdentifier identifier)
        {
            if (identifier == null)
            {
                return null;
            }

            var listDTO = _twitterListFactoryQueryExecutor.GetExistingList(identifier);
            return CreateListFromDTO(listDTO);
        }

        // Generate List from DTO
        public ITwitterList CreateListFromDTO(ITwitterListDTO twitterListDTO)
        {
            if (twitterListDTO == null)
            {
                return null;
            }

            var parameterOverride = _twitterListFactory.GenerateParameterOverrideWrapper("twitterListDTO", twitterListDTO);
            return _twitterListFactory.Create(parameterOverride);
        }

        public IEnumerable<ITwitterList> CreateListsFromDTOs(IEnumerable<ITwitterListDTO> listDTOs)
        {
            if (listDTOs == null)
            {
                return null;
            }

            return listDTOs.Select(CreateListFromDTO).ToArray();
        }

        public ITwitterList GenerateListFromJson(string json)
        {
            var listDTO = _jsonObjectConverter.DeserializeObject<ITwitterListDTO>(json);
            return CreateListFromDTO(listDTO);
        }
    }
}