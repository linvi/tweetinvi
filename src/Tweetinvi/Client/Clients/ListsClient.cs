using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class ListsClient : IListsClient
    {
        private readonly ITwitterListsRequester _twitterListsRequester;
        private readonly ITwitterClient _client;

        public ListsClient(
            ITwitterListsRequester twitterListsRequester,
            ITwitterClient client)
        {
            _twitterListsRequester = twitterListsRequester;
            _client = client;
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
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList> GetList(long listId)
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
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList[]> GetListsSubscribedByAccount()
        {
            return GetListsSubscribedByAccount(new GetListsSubscribedByAccountParameters());
        }

        public Task<ITwitterList[]> GetListsSubscribedByAccount(IGetListsSubscribedByAccountParameters parameters)
        {
            return GetListsSubscribedByUser(new GetListsSubscribedByUserParameters(parameters));
        }

        public Task<ITwitterList[]> GetListsSubscribedByUser(long userId)
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
            return _client.Factories.CreateTwitterLists(twitterResult?.DataTransferObject);
        }

        public async Task<ITwitterList> UpdateList(IUpdateListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.UpdateList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList> DestroyList(long listId)
        {
            return DestroyList(new DestroyListParameters(listId));
        }

        public Task<ITwitterList> DestroyList(string slug, IUserIdentifier user)
        {
            return DestroyList(new DestroyListParameters(slug, user));
        }

        public Task<ITwitterList> DestroyList(ITwitterListIdentifier list)
        {
            return DestroyList(new DestroyListParameters(list));
        }

        public async Task<ITwitterList> DestroyList(IDestroyListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.DestroyList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList[]> GetListsOwnedByAccount()
        {
            return GetListsOwnedByAccount(new GetListsOwnedByAccountParameters());
        }

        public async Task<ITwitterList[]> GetListsOwnedByAccount(IGetListsOwnedByAccountParameters parameters)
        {
            var iterator = GetListsOwnedByAccountIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator()
        {
            return GetListsOwnedByAccountIterator(new GetListsOwnedByAccountParameters());
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters parameters)
        {
            return GetListsOwnedByUserIterator(new GetListsOwnedByAccountByUserParameters(parameters));
        }

        public Task<ITwitterList[]> GetListsOwnedByUser(long userId)
        {
            return GetListsOwnedByUser(new GetListsOwnedByAccountByUserParameters(userId));
        }

        public Task<ITwitterList[]> GetListsOwnedByUser(string username)
        {
            return GetListsOwnedByUser(new GetListsOwnedByAccountByUserParameters(username));
        }

        public Task<ITwitterList[]> GetListsOwnedByUser(IUserIdentifier user)
        {
            return GetListsOwnedByUser(new GetListsOwnedByAccountByUserParameters(user));
        }

        public async Task<ITwitterList[]> GetListsOwnedByUser(IGetListsOwnedByUserParameters parameters)
        {
            var iterator = GetListsOwnedByUserIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(long userId)
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
                var listDtos = pageResult?.DataTransferObject?.TwitterLists;
                return listDtos?.Select(dto => _client.Factories.CreateTwitterList(dto)).ToArray();
            });
        }

        public Task<ITwitterList> AddMemberToList(long listId, long userId)
        {
            return AddMemberToList(new TwitterListIdentifier(listId), userId);
        }

        public Task<ITwitterList> AddMemberToList(ITwitterListIdentifier list, long userId)
        {
            return AddMemberToList(new AddMemberToListParameters(list, userId));
        }

        public Task<ITwitterList> AddMemberToList(ITwitterListIdentifier list, string username)
        {
            return AddMemberToList(new AddMemberToListParameters(list, username));
        }

        public Task<ITwitterList> AddMemberToList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return AddMemberToList(new AddMemberToListParameters(list, user));
        }

        public async Task<ITwitterList> AddMemberToList(IAddMemberToListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.AddMemberToList(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task<ITwitterList> AddMembersToList(long listId, IEnumerable<long> userIds)
        {
            return AddMembersToList(new AddMembersToListParameters(listId, userIds));
        }

        public Task<ITwitterList> AddMembersToList(long listId, IEnumerable<string> usernames)
        {
            return AddMembersToList(new AddMembersToListParameters(listId, usernames));
        }

        public Task<ITwitterList> AddMembersToList(long listId, IEnumerable<IUserIdentifier> users)
        {
            return AddMembersToList(new AddMembersToListParameters(listId, users));
        }

        public Task<ITwitterList> AddMembersToList(ITwitterListIdentifier list, IEnumerable<long> userIds)
        {
            return AddMembersToList(new AddMembersToListParameters(list, userIds));
        }

        public Task<ITwitterList> AddMembersToList(ITwitterListIdentifier list, IEnumerable<string> usernames)
        {
            return AddMembersToList(new AddMembersToListParameters(list, usernames));
        }

        public Task<ITwitterList> AddMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return AddMembersToList(new AddMembersToListParameters(list, users));
        }

        public async Task<ITwitterList> AddMembersToList(IAddMembersToListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.AddMembersToList(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task<ITwitterList[]> GetAccountListMemberships()
        {
            return GetAccountListMemberships(new GetAccountListMembershipsParameters());
        }

        public async Task<ITwitterList[]> GetAccountListMemberships(IGetAccountListMembershipsParameters parameters)
        {
            var iterator = GetAccountListMembershipsIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITwitterList> GetAccountListMembershipsIterator()
        {
            return GetAccountListMembershipsIterator(new GetAccountListMembershipsParameters());
        }

        public ITwitterIterator<ITwitterList> GetAccountListMembershipsIterator(IGetAccountListMembershipsParameters parameters)
        {
            return GetUserListMembershipsIterator(new GetUserListMembershipsParameters(parameters));
        }

        public Task<ITwitterList[]> GetUserListMemberships(long userId)
        {
            return GetUserListMemberships(new GetUserListMembershipsParameters(userId));
        }

        public Task<ITwitterList[]> GetUserListMemberships(string username)
        {
            return GetUserListMemberships(new GetUserListMembershipsParameters(username));
        }

        public Task<ITwitterList[]> GetUserListMemberships(IUserIdentifier user)
        {
            return GetUserListMemberships(new GetUserListMembershipsParameters(user));
        }

        public async Task<ITwitterList[]> GetUserListMemberships(IGetUserListMembershipsParameters parameters)
        {
            var iterator = GetUserListMembershipsIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(long userId)
        {
            return GetUserListMembershipsIterator(new GetUserListMembershipsParameters(userId));
        }

        public ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(string username)
        {
            return GetUserListMembershipsIterator(new GetUserListMembershipsParameters(username));
        }

        public ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(IUserIdentifier user)
        {
            return GetUserListMembershipsIterator(new GetUserListMembershipsParameters(user));
        }

        public ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters)
        {
            var iterator = _twitterListsRequester.GetUserListMembershipsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITwitterListCursorQueryResultDTO>, ITwitterList>(iterator, pageResult =>
            {
                var listDtos = pageResult.DataTransferObject.TwitterLists;
                return listDtos?.Select(dto => _client.Factories.CreateTwitterList(dto)).ToArray();
            });
        }

        public Task<IUser[]> GetMembersOfList(long listId)
        {
            return GetMembersOfList(new GetMembersOfListParameters(listId));
        }

        public Task<IUser[]> GetMembersOfList(ITwitterListIdentifier list)
        {
            return GetMembersOfList(new GetMembersOfListParameters(list));
        }

        public async Task<IUser[]> GetMembersOfList(IGetMembersOfListParameters parameters)
        {
            var iterator = GetMembersOfListIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IUser> GetMembersOfListIterator(long listId)
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
                return _client.Factories.CreateUsers(pageResult?.DataTransferObject?.Users);
            });
        }

        public Task<bool> CheckIfUserIsMemberOfList(long listId, long userId)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, userId));
        }

        public Task<bool> CheckIfUserIsMemberOfList(long listId, string username)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, username));
        }

        public Task<bool> CheckIfUserIsMemberOfList(long listId, IUserIdentifier user)
        {
            return CheckIfUserIsMemberOfList(new CheckIfUserIsMemberOfListParameters(listId, user));
        }

        public Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, long userId)
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
                await _twitterListsRequester.CheckIfUserIsAListMember(parameters).ConfigureAwait(false);
                return true;
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

        public Task<ITwitterList> RemoveMemberFromList(long listId, long userId)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, userId));
        }

        public Task<ITwitterList> RemoveMemberFromList(long listId, string username)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, username));
        }

        public Task<ITwitterList> RemoveMemberFromList(long listId, IUserIdentifier user)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(listId, user));
        }

        public Task<ITwitterList> RemoveMemberFromList(ITwitterListIdentifier list, long userId)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, userId));
        }

        public Task<ITwitterList> RemoveMemberFromList(ITwitterListIdentifier list, string username)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, username));
        }

        public Task<ITwitterList> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return RemoveMemberFromList(new RemoveMemberFromListParameters(list, user));
        }

        public async Task<ITwitterList> RemoveMemberFromList(IRemoveMemberFromListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.RemoveMemberFromList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList> RemoveMembersFromList(long listId, IEnumerable<long> userIds)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(listId, userIds));
        }

        public Task<ITwitterList> RemoveMembersFromList(long listId, IEnumerable<string> usernames)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(listId, usernames));
        }

        public Task<ITwitterList> RemoveMembersFromList(long listId, IEnumerable<IUserIdentifier> users)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(listId, users));
        }

        public Task<ITwitterList> RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(list, userIds));
        }

        public Task<ITwitterList> RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<string> usernames)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(list, usernames));
        }

        public Task<ITwitterList> RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return RemoveMembersFromList(new RemoveMembersFromListParameters(list, users));
        }

        public async Task<ITwitterList> RemoveMembersFromList(IRemoveMembersFromListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.RemoveMembersFromList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        // ***********
        // SUBSCRIBERS
        // ***********

        public Task<ITwitterList> SubscribeToList(long listId)
        {
            return SubscribeToList(new SubscribeToListParameters(listId));
        }

        public Task<ITwitterList> SubscribeToList(ITwitterListIdentifier list)
        {
            return SubscribeToList(new SubscribeToListParameters(list));
        }

        public async Task<ITwitterList> SubscribeToList(ISubscribeToListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.SubscribeToList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<ITwitterList> UnsubscribeFromList(long listId)
        {
            return UnsubscribeFromList(new UnsubscribeFromListParameters(listId));
        }

        public Task<ITwitterList> UnsubscribeFromList(ITwitterListIdentifier list)
        {
            return UnsubscribeFromList(new UnsubscribeFromListParameters(list));
        }

        public async Task<ITwitterList> UnsubscribeFromList(IUnsubscribeFromListParameters parameters)
        {
            var twitterResult = await _twitterListsRequester.UnsubscribeFromList(parameters).ConfigureAwait(false);
            return _client.Factories.CreateTwitterList(twitterResult?.DataTransferObject);
        }

        public Task<IUser[]> GetListSubscribers(long listId)
        {
            return GetListSubscribers(new GetListSubscribersParameters(listId));
        }

        public Task<IUser[]> GetListSubscribers(ITwitterListIdentifier list)
        {
            return GetListSubscribers(new GetListSubscribersParameters(list));
        }

        public async Task<IUser[]> GetListSubscribers(IGetListSubscribersParameters parameters)
        {
            var iterator = GetListSubscribersIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IUser> GetListSubscribersIterator(long listId)
        {
            return GetListSubscribersIterator(new GetListSubscribersParameters(listId));
        }

        public ITwitterIterator<IUser> GetListSubscribersIterator(ITwitterListIdentifier list)
        {
            return GetListSubscribersIterator(new GetListSubscribersParameters(list));
        }

        public ITwitterIterator<IUser> GetListSubscribersIterator(IGetListSubscribersParameters parameters)
        {
            var pageIterator = _twitterListsRequester.GetListSubscribersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(pageIterator, pageResult =>
            {
                return _client.Factories.CreateUsers(pageResult?.DataTransferObject?.Users);
            });
        }

        public ITwitterIterator<ITwitterList> GetAccountListSubscriptionsIterator(IGetAccountListSubscriptionsParameters parameters)
        {
            return GetUserListSubscriptionsIterator(new GetUserListSubscriptionsParameters(parameters));
        }

        public Task<ITwitterList[]> GetUserListSubscriptions(long userId)
        {
            return GetUserListSubscriptions(new GetUserListSubscriptionsParameters(userId));
        }

        public Task<ITwitterList[]> GetUserListSubscriptions(string username)
        {
            return GetUserListSubscriptions(new GetUserListSubscriptionsParameters(username));
        }

        public Task<ITwitterList[]> GetUserListSubscriptions(IUserIdentifier user)
        {
            return GetUserListSubscriptions(new GetUserListSubscriptionsParameters(user));
        }

        public async Task<ITwitterList[]> GetUserListSubscriptions(IGetUserListSubscriptionsParameters parameters)
        {
            var iterator = GetUserListSubscriptionsIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(long userId)
        {
            return GetUserListSubscriptionsIterator(new GetUserListSubscriptionsParameters(userId));
        }

        public ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(string username)
        {
            return GetUserListSubscriptionsIterator(new GetUserListSubscriptionsParameters(username));
        }

        public ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(IUserIdentifier user)
        {
            return GetUserListSubscriptionsIterator(new GetUserListSubscriptionsParameters(user));
        }

        public ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters)
        {
            var pageIterator = _twitterListsRequester.GetUserListSubscriptionsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITwitterListCursorQueryResultDTO>, ITwitterList>(pageIterator, pageResult =>
            {
                var twitterListDtos = pageResult.DataTransferObject.TwitterLists;
                return _client.Factories.CreateTwitterLists(twitterListDtos);
            });
        }

        Task<bool> IListsClient.CheckIfUserIsSubscriberOfList(long listId, long userId)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(listId, userId));
        }

        public Task<bool> CheckIfUserIsSubscriberOfList(long listId, string username)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(listId, username));
        }

        public Task<bool> CheckIfUserIsSubscriberOfList(long listId, IUserIdentifier user)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(listId, user));
        }

        public Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, long userId)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(list, userId));
        }

        public Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, string username)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(list, username));
        }

        public Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return CheckIfUserIsSubscriberOfList(new CheckIfUserIsSubscriberOfListParameters(list, user));
        }

        public async Task<bool> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters)
        {
            try
            {
                await _twitterListsRequester.CheckIfUserIsSubscriberOfList(parameters).ConfigureAwait(false);
                return true;
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

        // ***********
        // GET TWEETS
        // ***********

        public Task<ITweet[]> GetTweetsFromList(long listId)
        {
            return GetTweetsFromList(new GetTweetsFromListParameters(listId));
        }

        public Task<ITweet[]> GetTweetsFromList(ITwitterListIdentifier list)
        {
            return GetTweetsFromList(new GetTweetsFromListParameters(list));
        }

        public async Task<ITweet[]> GetTweetsFromList(IGetTweetsFromListParameters parameters)
        {
            var iterator = GetTweetsFromListIterator(parameters);
            return (await iterator.NextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(long listId)
        {
            return GetTweetsFromListIterator(new GetTweetsFromListParameters(listId));
        }

        public ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(ITwitterListIdentifier list)
        {
            return GetTweetsFromListIterator(new GetTweetsFromListParameters(list));
        }

        public ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters)
        {
            var pageIterator = _twitterListsRequester.GetTweetsFromListIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult => _client.Factories.CreateTweets(twitterResult?.DataTransferObject));
        }
    }
}