using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Factories.Friendship
{
    public class FriendshipFactory : IFriendshipFactory
    {
        private readonly IFactory<IRelationshipDetails> _unityRelationshipFactory;
        private readonly IFactory<IRelationshipState> _unityRelationshipStateFactory;
        private readonly IFactory<IFriendshipAuthorizations> _friendshipAuthorizationUnityFactory;


        public FriendshipFactory(
            IFactory<IRelationshipDetails> unityRelationshipFactory,
            IFactory<IRelationshipState> unityRelationshipStateFactory,
            IFactory<IFriendshipAuthorizations> friendshipAuthorizationUnityFactory)
        {
            _unityRelationshipFactory = unityRelationshipFactory;
            _unityRelationshipStateFactory = unityRelationshipStateFactory;
            _friendshipAuthorizationUnityFactory = friendshipAuthorizationUnityFactory;
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