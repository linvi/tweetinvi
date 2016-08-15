using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Friendship
{
    public class FriendshipFactory : IFriendshipFactory
    {
        private readonly IFactory<IRelationshipDetails> _unityRelationshipFactory;
        private readonly IFactory<IRelationshipState> _unityRelationshipStateFactory;
        private readonly IFactory<IFriendshipAuthorizations> _friendshipAuthorizationUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;


        public FriendshipFactory(
            IFactory<IRelationshipDetails> unityRelationshipFactory,
            IFactory<IRelationshipState> unityRelationshipStateFactory,
            IFactory<IFriendshipAuthorizations> friendshipAuthorizationUnityFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _unityRelationshipFactory = unityRelationshipFactory;
            _unityRelationshipStateFactory = unityRelationshipStateFactory;
            _friendshipAuthorizationUnityFactory = friendshipAuthorizationUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Generate From DTO
            
        public IRelationshipDetails GenerateRelationshipFromRelationshipDTO(IRelationshipDetailsDTO relationshipDetailsDTO)
        {
            if (relationshipDetailsDTO == null)
            {
                return null;
            }

            var relationshipParameter = _unityRelationshipFactory.GenerateParameterOverrideWrapper("relationshipDetailsDTO", relationshipDetailsDTO);
            return _unityRelationshipFactory.Create(relationshipParameter);
        }

        public IRelationshipDetails GenerateFriendshipDetailsFromJson(string json)
        {
            var dto = _jsonObjectConverter.DeserializeObject<IRelationshipDetailsDTO>(json);
            return GenerateRelationshipFromRelationshipDTO(dto);
        }

        public IRelationshipState GenerateFriendshipStateFromJson(string json)
        {
            var dto = _jsonObjectConverter.DeserializeObject<IRelationshipStateDTO>(json);
            return GenerateRelationshipStateFromRelationshipStateDTO(dto);
        }

        public IEnumerable<IRelationshipDetails> GenerateRelationshipsFromRelationshipsDTO(IEnumerable<IRelationshipDetailsDTO> relationshipDTOs)
        {
            if (relationshipDTOs == null)
            {
                return null;
            }

            return relationshipDTOs.Select(GenerateRelationshipFromRelationshipDTO).ToList();
        }

        // Generate Relationship state from DTO
        public IRelationshipState GenerateRelationshipStateFromRelationshipStateDTO(IRelationshipStateDTO relationshipStateDTO)
        {
            if (relationshipStateDTO == null)
            {
                return null;
            }

            var relationshipStateParameter = _unityRelationshipFactory.GenerateParameterOverrideWrapper("relationshipStateDTO", relationshipStateDTO);
            return _unityRelationshipStateFactory.Create(relationshipStateParameter);
        }

        public List<IRelationshipState> GenerateRelationshipStatesFromRelationshipStatesDTO(IEnumerable<IRelationshipStateDTO> relationshipStateDTOs)
        {
            if (relationshipStateDTOs == null)
            {
                return null;
            }

            return relationshipStateDTOs.Select(GenerateRelationshipStateFromRelationshipStateDTO).ToList();
        }

        // Generate RelationshipAuthorizations
        public IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool deviceNotificationEnabled)
        {
            var friendshipAuthorization = _friendshipAuthorizationUnityFactory.Create();

            friendshipAuthorization.RetweetsEnabled = retweetsEnabled;
            friendshipAuthorization.DeviceNotificationEnabled = deviceNotificationEnabled;

            return friendshipAuthorization;
        }
    }
}