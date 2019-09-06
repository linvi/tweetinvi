using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Logic
{
    public class TwitterList : ITwitterList
    {
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListController _twitterListController;
        private ITwitterListDTO _twitterListDTO;
        private IUser _owner;

        public TwitterList(
            IUserFactory userFactory,
            ITwitterListController twitterListController,
            ITwitterListDTO twitterListDTO)
        {
            _userFactory = userFactory;
            _twitterListController = twitterListController;
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

        public long? OwnerId
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

        public Task<IEnumerable<ITweet>> GetTweets(IGetTweetsFromListParameters getTweetsFromListParameters = null)
        {
            return _twitterListController.GetTweetsFromList(this, getTweetsFromListParameters);
        }

        // Members
        public Task<IEnumerable<IUser>> GetMembers(int maximumNumberOfUsersToRetrieve = 100)
        {
            return _twitterListController.GetListMembers(_twitterListDTO, maximumNumberOfUsersToRetrieve);
        }


        public Task<bool> AddMember(long userId)
        {
            return _twitterListController.AddMemberToList(this, userId);
        }

        public Task<bool> AddMember(string userScreenName)
        {
            return _twitterListController.AddMemberToList(this, userScreenName);
        }

        public Task<bool> AddMember(IUserIdentifier user)
        {
            return _twitterListController.AddMemberToList(this, user);
        }

        public Task<MultiRequestsResult> AddMultipleMembers(IEnumerable<long> userIds)
        {
            return _twitterListController.AddMultipleMembersToList(this, userIds);
        }

        public Task<MultiRequestsResult> AddMultipleMembers(IEnumerable<string> userScreenNames)
        {
            return _twitterListController.AddMultipleMembersToList(this, userScreenNames);
        }

        public Task<MultiRequestsResult> AddMultipleMembers(IEnumerable<IUserIdentifier> users)
        {
            return _twitterListController.AddMultipleMembersToList(this, users);
        }


        public Task<bool> RemoveMember(long userId)
        {
            return _twitterListController.RemoveMemberFromList(this, userId);
        }

        public Task<bool> RemoveMember(string userScreenName)
        {
            return _twitterListController.RemoveMemberFromList(this, userScreenName);
        }

        public Task<bool> RemoveMember(IUserIdentifier user)
        {
            return _twitterListController.RemoveMemberFromList(this, user);
        }


        public Task<MultiRequestsResult> RemoveMultipleMembers(IEnumerable<long> userIds)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, userIds);
        }

        public Task<MultiRequestsResult> RemoveMultipleMembers(IEnumerable<string> userScreenNames)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, userScreenNames);
        }

        public Task<MultiRequestsResult> RemoveMultipleMembers(IEnumerable<IUserIdentifier> users)
        {
            return _twitterListController.RemoveMultipleMembersFromList(this, users);
        }


        public Task<bool> CheckUserMembership(long userId)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, userId);
        }

        public Task<bool> CheckUserMembership(string userScreenName)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, userScreenName);
        }

        public Task<bool> CheckUserMembership(IUserIdentifier user)
        {
            return _twitterListController.CheckIfUserIsAListMember(this, user);
        }

        // Subscribers
        public Task<IEnumerable<IUser>> GetSubscribers(int maximumNumberOfUsersToRetrieve = 100)
        {
            return _twitterListController.GetListSubscribers(this, maximumNumberOfUsersToRetrieve);
        }

        public Task<bool> SubscribeAuthenticatedUserToList(IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.SubscribeToList(this);
            }

            return _twitterListController.SubscribeAuthenticatedUserToList(this);
        }

        public Task<bool> UnSubscribeAuthenticatedUserFromList(IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(this);
            }

            return _twitterListController.UnSubscribeAuthenticatedUserFromList(this);
        }

        
        public Task<bool> CheckUserSubscription(long userId)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, userId);
        }

        public Task<bool> CheckUserSubscription(string userScreenName)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, userScreenName);
        }

        public Task<bool> CheckUserSubscription(IUserIdentifier user)
        {
            return _twitterListController.CheckIfUserIsAListSubscriber(this, user);
        }


        public async Task<bool> Update(ITwitterListUpdateParameters parameters)
        {
            var updateList = await _twitterListController.UpdateList(this, parameters);

            if (updateList != null)
            {
                _twitterListDTO = updateList.TwitterListDTO;
                return true;
            }

            return false;
        }

        public Task<bool> Destroy()
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
    }
}