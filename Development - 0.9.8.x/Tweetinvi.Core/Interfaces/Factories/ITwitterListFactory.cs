using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ITwitterListFactory
    {
        ITwitterList CreateList(string name, PrivacyMode privacyMode, string description);

        ITwitterList GetExistingList(ITwitterListIdentifier listIdentifier);
        ITwitterList GetExistingList(long listId);
        ITwitterList GetExistingList(string slug, IUser user);
        ITwitterList GetExistingList(string slug, IUserIdentifier userDTO);
        ITwitterList GetExistingList(string slug, long userId);
        ITwitterList GetExistingList(string slug, string userScreenName);

        ITwitterList CreateListFromDTO(ITwitterListDTO twitterListDTO);
        IEnumerable<ITwitterList> CreateListsFromDTOs(IEnumerable<ITwitterListDTO> listDTOs);
        
        ITwitterList CreateListFromJson(string jsonList);
    }
}