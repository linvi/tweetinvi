using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    public class TwitterList : ITwitterList
    {
        private readonly ITwitterListController _twitterListController;
        private ITwitterListDTO _twitterListDTO;
        private IUser _owner;

        public TwitterList(ITwitterListDTO twitterListDTO, ITwitterClient client)
        {
            // ! order is important, client should be at the top so that `UpdateOwner`
            // can use the client factories to create the owner user.
            Client = client;

            _twitterListDTO = twitterListDTO;
            UpdateOwner();
        }

        public ITwitterListDTO TwitterListDTO
        {
            get => _twitterListDTO;
            set
            {
                _twitterListDTO = value;
                UpdateOwner();
            }
        }

        public ITwitterClient Client { get; }

        public long Id => _twitterListDTO.Id;
        public string IdStr => _twitterListDTO.IdStr;
        public string Slug => _twitterListDTO.Slug;
        public long? OwnerId => _twitterListDTO.OwnerId;
        public string OwnerScreenName => _twitterListDTO.OwnerScreenName;
        public string Name => _twitterListDTO.Name;
        public string FullName => _twitterListDTO.FullName;
        public IUser Owner => _owner;
        public DateTime CreatedAt => _twitterListDTO.CreatedAt;
        public string Uri => _twitterListDTO.Uri;
        public string Description => _twitterListDTO.Description;
        public bool Following => _twitterListDTO.Following;
        public PrivacyMode PrivacyMode => _twitterListDTO.PrivacyMode;
        public int MemberCount => _twitterListDTO.MemberCount;
        public int SubscriberCount => _twitterListDTO.SubscriberCount;

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
                _owner = Client.Factories.CreateUser(_twitterListDTO.Owner);
            }
        }
    }
}