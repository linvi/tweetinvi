using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListController : ITwitterListController
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListQueryExecutor _twitterListQueryExecutor;
        private readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;
        private readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;

        public TwitterListController(
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITwitterListQueryExecutor twitterListQueryExecutor,
            ITwitterListQueryParameterGenerator twitterListQueryParameterGenerator,
            ITwitterListIdentifierFactory twitterListIdentifierFactory)
        {
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _twitterListQueryExecutor = twitterListQueryExecutor;
            _twitterListQueryParameterGenerator = twitterListQueryParameterGenerator;
            _twitterListIdentifierFactory = twitterListIdentifierFactory;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CreateList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetListsSubscribedByUser(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.UpdateList(parameters, request);
        }

        Task<ITwitterResult<ITwitterListDTO>> ITwitterListController.DestroyList(IDestroyListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.DestroyList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetListsOwnedByAccountByUserParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _twitterListQueryExecutor.GetListsOwnedByUser(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMemberToList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMembersToList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetListsAUserIsMemberOfParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _twitterListQueryExecutor.GetListsAUserIsMemberOf(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMembersOfListParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _twitterListQueryExecutor.GetMembersOfList(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListMember(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMemberFromList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMembersFromList(parameters, request);
        }


        #region Get Tweets from List
        public Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetTweetsFromList(identifier);
        }

        public Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return GetTweetsFromList(identifier);
        }

        public Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return GetTweetsFromList(identifier);
        }

        public Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return GetTweetsFromList(identifier);
        }

        public Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null)
        {
            var queryParameters = _twitterListQueryParameterGenerator.CreateTweetsFromListQueryParameters(list, parameters);
            return GetTweetsFromList(queryParameters);
        }

        private async Task<IEnumerable<ITweet>> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters)
        {
            var tweetsDTO = await _twitterListQueryExecutor.GetTweetsFromList(queryParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO, null, null);
        }
        #endregion

        #region GetUserSubscribedLists
        public Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve)
        {
            return GetUserSubscribedLists(new UserIdentifier(userId), maxNumberOfListsToRetrieve);
        }

        public Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(string userName, int maxNumberOfListsToRetrieve)
        {
            return GetUserSubscribedLists(new UserIdentifier(userName), maxNumberOfListsToRetrieve);
        }

        public async Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve)
        {
            var listDTOs = await _twitterListQueryExecutor.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
            return null;
        }
        #endregion

        #region Get list subscribers
        public Task<IEnumerable<IUser>> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public Task<IEnumerable<IUser>> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public Task<IEnumerable<IUser>> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public Task<IEnumerable<IUser>> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public async Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            var usersDTO = await _twitterListQueryExecutor.GetListSubscribers(list, maximumNumberOfUsersToRetrieve);
            return _userFactory.GenerateUsersFromDTO(usersDTO, null);
        }
        #endregion

        #region Add subscriber to List

        public Task<bool> SubscribeAuthenticatedUserToList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public Task<bool> SubscribeAuthenticatedUserToList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public Task<bool> SubscribeAuthenticatedUserToList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public Task<bool> SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier list)
        {
            return _twitterListQueryExecutor.SubscribeAuthenticatedUserToList(list);
        }

        #endregion

        #region UnSubscribeAuthenticatedUserFromList
        public Task<bool> UnSubscribeAuthenticatedUserFromList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier list)
        {
            return _twitterListQueryExecutor.UnSubscribeAuthenticatedUserFromList(list);
        }
        #endregion

        #region CheckIfUserIsAListSubscriber
        public Task<bool> CheckIfUserIsAListSubscriber(long listId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(long listId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(long listId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, long userId)
        {
            return CheckIfUserIsAListSubscriber(listIdentifier, new UserIdentifier(userId));
        }

        public Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, string userScreenName)
        {
            return CheckIfUserIsAListSubscriber(listIdentifier, new UserIdentifier(userScreenName));
        }

        public Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListSubscriber(listIdentifier, user);
        }



        #endregion
    }
}