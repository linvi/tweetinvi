using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ITwitterListFactory
    {
        ITwitterList CreateList(string name, PrivacyMode privacyMode, string description);

        ITwitterList GetExistingList(ITwitterListIdentifier listIdentifier);
        ITwitterList GetExistingList(long listId);
        ITwitterList GetExistingList(string slug, IUser user);
        ITwitterList GetExistingList(string slug, IUserIdentifier userIdentifier);
        ITwitterList GetExistingList(string slug, long userId);
        ITwitterList GetExistingList(string slug, string userScreenName);

        ITwitterList CreateListFromDTO(ITwitterListDTO twitterListDTO);
        IEnumerable<ITwitterList> CreateListsFromDTOs(IEnumerable<ITwitterListDTO> listDTOs);
        
        ITwitterList GenerateListFromJson(string json);
    }
}