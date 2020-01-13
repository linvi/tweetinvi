using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ITwitterListFactory
    {
        Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode, string description);

        Task<ITwitterList> GetExistingList(ITwitterListIdentifier listIdentifier);
        Task<ITwitterList> GetExistingList(long listId);
        Task<ITwitterList> GetExistingList(string slug, IUserIdentifier user);
        Task<ITwitterList> GetExistingList(string slug, long userId);
        Task<ITwitterList> GetExistingList(string slug, string userScreenName);

        IEnumerable<ITwitterList> CreateListsFromDTOs(IEnumerable<ITwitterListDTO> listDTOs);
    }
}