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
        Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyListAsync(IDestroyListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request);

        // MEMBERS
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToListAsync(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToListAsync(IAddMembersToListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters, ITwitterRequest request);

        // SUBSCRIBERS
        Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request);

        // GET TWEETS
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters, ITwitterRequest request);
    }
}