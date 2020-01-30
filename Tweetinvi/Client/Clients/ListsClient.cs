using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client
{
    public class ListsClient : IListsClient
    {
        private readonly ITwitterListsRequester _twitterListsRequester;
        private readonly IUserFactory _userFactory;

        public ListsClient(
            ITwitterListsRequester twitterListsRequester,
            IUserFactory userFactory)
        {
            _twitterListsRequester = twitterListsRequester;
            _userFactory = userFactory;
        }

        public Task<ITwitterList> CreateList(string name)
        {
            return CreateList(new CreateListParameters(name));
        }

        public Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode)
        {
            return CreateList(new CreateListParameters(name)
            {
                PrivacyMode = privacyMode
            });
        }

        public async Task<ITwitterList> CreateList(ICreateListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.CreateList(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task<ITwitterList> GetList(long? listId)
        {
            return GetList(new GetListParameters(listId));
        }

        public Task<ITwitterList> GetList(string slug, IUserIdentifier user)
        {
            return GetList(new GetListParameters(slug, user));
        }

        public Task<ITwitterList> GetList(ITwitterListIdentifier listId)
        {
            return GetList(new GetListParameters(listId));
        }

        public async Task<ITwitterList> GetList(IGetListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.GetList(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task<ITwitterList[]> GetUserLists()
        {
            return GetUserLists(new GetUserListsParameters());
        }

        public Task<ITwitterList[]> GetUserLists(long? userId)
        {
            return GetUserLists(new GetUserListsParameters(userId));
        }

        public Task<ITwitterList[]> GetUserLists(string username)
        {
            return GetUserLists(new GetUserListsParameters(username));
        }

        public Task<ITwitterList[]> GetUserLists(IUserIdentifier user)
        {
            return GetUserLists(new GetUserListsParameters(user));
        }

        public async Task<ITwitterList[]> GetUserLists(IGetUserListsParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.GetUserLists(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public async Task<ITwitterList> UpdateList(IUpdateListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.UpdateList(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task DestroyList(long? listId)
        {
            return DestroyList(new DestroyListParameters(listId));
        }

        public Task DestroyList(string slug, IUserIdentifier user)
        {
            return DestroyList(new DestroyListParameters(slug, user));
        }

        public Task DestroyList(ITwitterListIdentifier listId)
        {
            return DestroyList(new DestroyListParameters(listId));
        }

        public async Task DestroyList(IDestroyListParameters parameters)
        {
            await _twitterListsRequester.DestroyList(parameters).ConfigureAwait(false);
        }

        public Task AddMemberToList(long? listId, long? userId)
        {
            return AddMemberToList(new TwitterListIdentifier(listId), userId);
        }

        public Task AddMemberToList(ITwitterListIdentifier list, long? userId)
        {
            return AddMemberToList(new AddMemberToListParameters(list, userId));
        }

        public Task AddMemberToList(ITwitterListIdentifier list, string username)
        {
            return AddMemberToList(new AddMemberToListParameters(list, username));
        }

        public Task AddMemberToList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return AddMemberToList(new AddMemberToListParameters(list, user));
        }

        public async Task AddMemberToList(IAddMemberToListParameters parameters)
        {
            await _twitterListsRequester.AddMemberToList(parameters).ConfigureAwait(false);
        }

        public ITwitterIterator<IUser> GetMembersOfListIterator(ITwitterListIdentifier list)
        {
            return GetMembersOfListIterator(new GetMembersOfListParameters(list));
        }

        public ITwitterIterator<IUser> GetMembersOfListIterator(IGetMembersOfListParameters parameters)
        {
            var iterator = _twitterListsRequester.GetMembersOfListIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(iterator, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _userFactory.GenerateUsersFromDTO(userDTOs, null);
            });
        }
    }
}