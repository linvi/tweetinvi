using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client
{
    public class ListsClient : IListsClient
    {
        private readonly ITwitterListsRequester _twitterListsRequester;
        private readonly ITwitterClientFactories _clientFactories;
        private readonly IUserFactory _userFactory;

        public ListsClient(
            ITwitterListsRequester twitterListsRequester,
            ITwitterClientFactories clientFactories,
            IUserFactory userFactory)
        {
            _twitterListsRequester = twitterListsRequester;
            _clientFactories = clientFactories;
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

        public Task<ITwitterList[]> GetListsSubscribedByAccount()
        {
            return GetListsSubscribedByAccount(new GetListsSubscribedByAccountParameters());
        }

        public Task<ITwitterList[]> GetListsSubscribedByAccount(IGetListsSubscribedByAccountParameters parameters)
        {
            return GetListsSubscribedByUser(new GetListsSubscribedByUserParameters(parameters));
        }

        public Task<ITwitterList[]> GetListsSubscribedByUser(long? userId)
        {
            return GetListsSubscribedByUser(new GetListsSubscribedByUserParameters(userId));
        }

        public Task<ITwitterList[]> GetListsSubscribedByUser(string username)
        {
            return GetListsSubscribedByUser(new GetListsSubscribedByUserParameters(username));
        }

        public Task<ITwitterList[]> GetListsSubscribedByUser(IUserIdentifier user)
        {
            return GetListsSubscribedByUser(new GetListsSubscribedByUserParameters(user));
        }

        public async Task<ITwitterList[]> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.GetListsSubscribedByUser(parameters).ConfigureAwait(false);
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

        public ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator()
        {
            return GetListsOwnedByAccountIterator(new GetListsOwnedByAccountParameters());
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters parameters)
        {
            return GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(parameters));
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(long? userId)
        {
            return GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(userId));
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(string username)
        {
            return GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(username));
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(IUser user)
        {
            return GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(user));
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters)
        {
            var iterator = _twitterListsRequester.GetListsOwnedByUserIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITwitterListCursorQueryResultDTO>, ITwitterList>(iterator, pageResult =>
            {
                var listDtos = pageResult.DataTransferObject.TwitterLists;
                return listDtos?.Select(dto => _clientFactories.CreateTwitterList(dto)).ToArray();
            });
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

        public ITwitterIterator<ITwitterList> GetListsAccountIsMemberOfIterator()
        {
            return GetListsAccountIsMemberOfIterator(new GetListsAccountIsMemberOfParameters());
        }

        public ITwitterIterator<ITwitterList> GetListsAccountIsMemberOfIterator(IGetListsAccountIsMemberOfParameters parameters)
        {
            return GetListsAUserIsMemberOfIterator(new GetListsAUserIsMemberOfParameters(parameters));
        }

        public ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(long? userId)
        {
            return GetListsAUserIsMemberOfIterator(new GetListsAUserIsMemberOfParameters(userId));
        }

        public ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(string username)
        {
            return GetListsAUserIsMemberOfIterator(new GetListsAUserIsMemberOfParameters(username));
        }

        public ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(IUserIdentifier user)
        {
            return GetListsAUserIsMemberOfIterator(new GetListsAUserIsMemberOfParameters(user));
        }

        public ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters parameters)
        {
            var iterator = _twitterListsRequester.GetListsAUserIsMemberOfIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITwitterListCursorQueryResultDTO>, ITwitterList>(iterator, pageResult =>
            {
                var listDtos = pageResult.DataTransferObject.TwitterLists;
                return listDtos?.Select(dto => _clientFactories.CreateTwitterList(dto)).ToArray();
            });
        }

        public ITwitterIterator<IUser> GetMembersOfListIterator(long? listId)
        {
            return GetMembersOfListIterator(new GetMembersOfListParameters(listId));
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

        public Task<bool> CheckIfUserIsMemberOfList(long? listId, long? userId)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, userId));
        }

        public Task<bool> CheckIfUserIsMemberOfList(long? listId, string username)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, username));
        }

        public Task<bool> CheckIfUserIsMemberOfList(long? listId, IUserIdentifier user)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, user));
        }

        public Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, long? userId)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(list, userId));
        }

        public Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, string username)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(list, username));
        }

        public Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(list, user));
        }

        public async Task<bool> CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters parameters)
        {
            try
            {
                var result = await _twitterListsRequester.CheckIfUserIsAListMember(parameters).ConfigureAwait(false);
                return result.Result;
            }
            catch (TwitterException e)
            {
                if (e.StatusCode == 404)
                {
                    // This is a special case where the request actually throws expectedly
                    // When a user is not a member of a list this operation returns a 404.
                    return false;
                }

                throw;
            }
        }

        public Task RemoveMemberFromList(long? listId, long? userId)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, userId));
        }

        public Task RemoveMemberFromList(long? listId, string username)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, username));
        }

        public Task RemoveMemberFromList(long? listId, IUserIdentifier user)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, user));
        }

        public Task RemoveMemberFromList(ITwitterListIdentifier list, long? userId)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, userId));
        }

        public Task RemoveMemberFromList(ITwitterListIdentifier list, string username)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, username));
        }

        public Task RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, user));
        }

        public Task RemoveMemberFromList(IRemoveMemberFromListParameters parameters)
        {
            return _twitterListsRequester.RemoveMemberFromList(parameters);
        }
    }
}