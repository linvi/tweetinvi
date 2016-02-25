using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Logic
{
    public class TwitterList : ITwitterList
    {
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListController _twitterListController;
        private readonly ITaskFactory _taskFactory;
        private ITwitterListDTO _twitterListDTO;
        private IUser _owner;

        public TwitterList(
            IUserFactory userFactory,
            ITwitterListController twitterListController,
            ITwitterListDTO twitterListDTO,
            ITaskFactory taskFactory)
        {
            _userFactory = userFactory;
            _twitterListController = twitterListController;
            _taskFactory = taskFactory;
            TwitterListDTO = twitterListDTO;
        }

        public ITwitterListDTO TwitterListDTO
        {
            get { return _twitterListDTO; }
            set
            {
                _twitterListDTO = value;
                UpdateOwner();
            }
        }

        public long Id
        {
            get { return _twitterListDTO.Id; }
        }

        public string IdStr
        {
            get { return _twitterListDTO.IdStr; }
        }

        public string Slug
        {
            get { return _twitterListDTO.Slug; }
        }

        public long OwnerId
        {
            get { return _twitterListDTO.OwnerId; }
        }

        public string OwnerScreenName
        {
            get { return _twitterListDTO.OwnerScreenName; }
        }

        public string Name
        {
            get { return _twitterListDTO.Name; }
        }

        public string FullName
        {
            get { return _twitterListDTO.FullName; }
        }

        public IUser Owner
        {
            get { return _owner; }
        }

        public DateTime CreatedAt
        {
            get { return _twitterListDTO.CreatedAt; }
        }

        public string Uri
        {
            get { return _twitterListDTO.Uri; }

        }

        public string Description
        {
            get { return _twitterListDTO.Description; }
        }

        public bool Following
        {
            get { return _twitterListDTO.Following; }
        }

        public PrivacyMode PrivacyMode
        {
            get { return _twitterListDTO.PrivacyMode; }
        }

        public int MemberCount
        {
            get { return _twitterListDTO.MemberCount; }
        }

        public int SubscriberCount
        {
            get { return _twitterListDTO.SubscriberCount; }
        }

        public IEnumerable<ITweet> GetTweets(IGetTweetsFromListParameters getTweetsFromListParameters = null)
        {
            return _twitterListController.GetTweetsFromList(this, getTweetsFromListParameters);
        }

        // Members
        public IEnumerable<IUser> GetMembers(int maximumNumberOfUsersToRetrieve = 100)
        {
            return _twitterListController.GetListMembers(_twitterListDTO, maximumNumberOfUsersToRetrieve);
        }


        public bool AddMember(long userId)
        {
            return _twitterListController.AddMemberToList(this, userId);
        }

        public bool AddMember(string userScreenName)
        {
            return _twitterListController.AddMemberToList(this, userScreenName);
        }

        public bool AddMember(IUserIdentifier user)
        {
            return _twitterListController.AddMemberToList(this, user);
        }

        public MultiRequestsResult AddMultipleMembers(IEnumerable<long> userIds)
        {
            return _twitterListController.AddMultipleMembersToList(this, userIds);
        }

        public MultiRequestsResult AddMultipleMembers(IEnumerable<string> userScreenNames)
        {
            return _twitterListController.AddMultipleMembersToList(this, userScreenNames);
        }

        public MultiRequestsResult AddMultipleMembers(IEnumerable<IUserIdentifier> users)
        {
            return _twitterListController.AddMultipleMembersToList(this, users);
        }


        public bool RemoveMember(long userId)
        {
            return _twitterListController.RemoveMemberFromList(this, userId);
        }

        public bool RemoveMember(string userScreenName)
        {
            return _twitterListController.RemoveMemberFromList(this, userScreenName);
        }

        public bool RemoveMember(IUserIdentifier user)
        {
            return _twitterListController.RemoveMemberFromList(this, user);
        }


        public MultiRequestsResult RemoveMultipleMembers(IEnumerable<long> userIds)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, userIds);
        }

        public MultiRequestsResult RemoveMultipleMembers(IEnumerable<string> userScreenNames)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, userScreenNames);
        }

        public MultiRequestsResult RemoveMultipleMembers(IEnumerable<IUserIdentifier> users)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, users);
        }


        public bool CheckUserMembership(long userId)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, userId);
        }

        public bool CheckUserMembership(string userScreenName)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, userScreenName);
        }

        public bool CheckUserMembership(IUserIdentifier user)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, user);
        }

        // Subscribers
        public IEnumerable<IUser> GetSubscribers(int maximumNumberOfUsersToRetrieve = 100)
        {
            return _twitterListController.GetListSubscribers(this, maximumNumberOfUsersToRetrieve);
        }

        public bool SubscribeAuthenticatedUserToList(IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.SubsribeToList(this);
            }

            return _twitterListController.SubscribeAuthenticatedUserToList(this);
        }

        public bool UnSubscribeAuthenticatedUserFromList(IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(this);
            }

            return _twitterListController.UnSubscribeAuthenticatedUserFromList(this);
        }

        
        public bool CheckUserSubscription(long userId)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, userId);
        }

        public bool CheckUserSubscription(string userScreenName)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, userScreenName);
        }

        public bool CheckUserSubscription(IUserIdentifier user)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, user);
        }


        public bool Update(ITwitterListUpdateParameters parameters)
        {
            var updateList = _twitterListController.UpdateList(this, parameters);

            if (updateList != null)
            {
                _twitterListDTO = updateList.TwitterListDTO;
                return true;
            }

            return false;
        }

        public bool Destroy()
        {
            return _twitterListController.DestroyList(_twitterListDTO);
        }

        private void UpdateOwner()
        {
            if (_twitterListDTO != null)
            {
                _owner = _userFactory.GenerateUserFromDTO(_twitterListDTO.Owner);
            }
        }

        #region Async

        public async Task<bool> UpdateAsync(ITwitterListUpdateParameters parameters)
        {
            return await _taskFactory.ExecuteTaskAsync(() => Update(parameters));
        }

        public async Task<bool> DestroyAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Destroy());
        } 

        public async Task<IEnumerable<ITweet>> GetTweetsAsync(IGetTweetsFromListParameters getTweetsFromListParameters = null)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetTweets(getTweetsFromListParameters));
        }

        // Membership Async
        public async Task<IEnumerable<IUser>> GetMembersAsync(int maxNumberOfUsersToRetrieve = 100)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetMembers(maxNumberOfUsersToRetrieve));
        }

        public async Task<bool> AddMemberAsync(IUserIdentifier user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => AddMember(user));
        }

        public async Task<MultiRequestsResult> AddMultipleMembersAsync(IEnumerable<IUserIdentifier> users)
        {
            return await _taskFactory.ExecuteTaskAsync(() => AddMultipleMembers(users));
        }

        public async Task<bool> RemoveMemberAsync(IUserIdentifier user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => RemoveMember(user));
        }

        public async Task<MultiRequestsResult> RemoveMultipleMembersAsync(IEnumerable<IUserIdentifier> users)
        {
            return await _taskFactory.ExecuteTaskAsync(() => RemoveMultipleMembers(users));
        }

        public async Task<bool> CheckUserMembershipAsync(IUserIdentifier user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => CheckUserMembership(user));
        }

        // Subscriptions Async
        public async Task<IEnumerable<IUser>> GetSubscribersAsync(int maximumNumberOfUsersToRetrieve = 100)
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetSubscribers(maximumNumberOfUsersToRetrieve));
        }

        public async Task<bool> SubscribeAuthenticatedUserToListAsync(IAuthenticatedUser authenticatedUser = null)
        {
            return await _taskFactory.ExecuteTaskAsync(() => SubscribeAuthenticatedUserToList(authenticatedUser));
        }

        public async Task<bool> UnSubscribeAuthenticatedUserFromListAsync(IAuthenticatedUser authenticatedUser = null)
        {
            return await _taskFactory.ExecuteTaskAsync(() => UnSubscribeAuthenticatedUserFromList(authenticatedUser));
        }

        public async Task<bool> CheckUserSubscriptionAsync(IUserIdentifier user)
        {
            return await _taskFactory.ExecuteTaskAsync(() => CheckUserSubscription(user));
        }
        
        #endregion
    }
}