using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface IFriendshipFactory
    {
        // Generate from DTO
        IRelationshipDetails GenerateRelationshipFromRelationshipDTO(IRelationshipDetailsDTO relationshipDetailsDTO);
        IEnumerable<IRelationshipDetails> GenerateRelationshipsFromRelationshipsDTO(IEnumerable<IRelationshipDetailsDTO> relationshipDTO);

        // Generate RelationshipAuthorizations
        IRelationshipDetails GenerateFriendshipDetailsFromJson(string json);
        IRelationshipState GenerateFriendshipStateFromJson(string json);
        IRelationshipState[] GenerateRelationshipStatesFromRelationshipStatesDTO(IEnumerable<IRelationshipStateDTO> relationshipStateDTOs);
        IRelationshipState GenerateRelationshipStateFromRelationshipStateDTO(IRelationshipStateDTO relationshipStateDTO);
    }
}