using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IFriendshipFactory
    {
        // Generate from DTO
        IRelationshipDetails GenerateRelationshipFromRelationshipDTO(IRelationshipDetailsDTO relationshipDetailsDTO);
        IEnumerable<IRelationshipDetails> GenerateRelationshipsFromRelationshipsDTO(IEnumerable<IRelationshipDetailsDTO> relationshipDTO);

        // Generate RelationshipAuthorizations
        IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool deviceNotificationEnabled);
    }
}