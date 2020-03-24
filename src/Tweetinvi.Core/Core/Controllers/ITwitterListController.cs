using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITwitterListController
    {
        // LIST
        Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request);

        // MEMBERS
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request);

        // SUBSCRIBERS
        Task<ITwitterResult<ITwitterListDTO>> SubscribeToList(ISubscribeToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromList(IUnsubscribeFromListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request);

        // GET TWEETS
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters, ITwitterRequest request);
    }
}