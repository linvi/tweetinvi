using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Friendship
{
    public class FriendshipFactory : IFriendshipFactory
    {
        private readonly IFactory<IRelationshipDetails> _unityRelationshipFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;


        public FriendshipFactory(
            IFactory<IRelationshipDetails> unityRelationshipFactory,
            IFactory<IRelationshipState> unityRelationshipStateFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _unityRelationshipFactory = unityRelationshipFactory;
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

            return new RelationshipState(relationshipStateDTO);
        }

        public IRelationshipState[] GenerateRelationshipStatesFromRelationshipStatesDTO(IEnumerable<IRelationshipStateDTO> relationshipStateDTOs)
        {
            if (relationshipStateDTOs == null)
            {
                return null;
            }

            return relationshipStateDTOs.Select(GenerateRelationshipStateFromRelationshipStateDTO).ToArray();
        }
    }
}