using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    public class TwitterList : ITwitterList
    {
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
        public long OwnerId => _twitterListDTO.OwnerId;
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

        public Task<ITweet[]> GetTweetsAsync()
        {
            return Client.Lists.GetTweetsFromListAsync(this);
        }

        // Members
        public Task<IUser[]> GetMembersAsync()
        {
            return Client.Lists.GetMembersOfListAsync(new GetMembersOfListParameters(this));
        }

        public Task AddMemberAsync(long userId)
        {
            return Client.Lists.AddMemberToListAsync(this, userId);
        }

        public Task AddMemberAsync(string username)
        {
            return Client.Lists.AddMemberToListAsync(this, username);
        }

        public Task AddMemberAsync(IUserIdentifier user)
        {
            return Client.Lists.AddMemberToListAsync(this, user);
        }

        public Task AddMembersAsync(IEnumerable<long> userIds)
        {
            return Client.Lists.AddMembersToListAsync(this, userIds);
        }

        public Task AddMembersAsync(IEnumerable<string> usernames)
        {
            return Client.Lists.AddMembersToListAsync(this, usernames);
        }

        public Task AddMembersAsync(IEnumerable<IUserIdentifier> users)
        {
            return Client.Lists.AddMembersToListAsync(this, users);
        }


        public Task<bool> RemoveMemberAsync(long userId)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, userId);
        }

        public Task<bool> RemoveMemberAsync(string username)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, username);
        }

        public Task<bool> RemoveMemberAsync(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, user);
        }


        public Task RemoveMembersAsync(IEnumerable<long> userIds)
        {
            return Client.Lists.RemoveMembersFromListAsync(this, userIds);
        }

        public Task RemoveMembersAsync(IEnumerable<string> usernames)
        {
            return Client.Lists.RemoveMembersFromListAsync(this, usernames);
        }

        public Task RemoveMembersAsync(IEnumerable<IUserIdentifier> users)
        {
            return Client.Lists.RemoveMembersFromListAsync(this, users);
        }


        public Task<bool> CheckUserMembershipAsync(long userId)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, userId);
        }

        public Task<bool> CheckUserMembershipAsync(string userScreenName)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, userScreenName);
        }

        public Task<bool> CheckUserMembershipAsync(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsMemberOfListAsync(this, user);
        }

        // Subscribers
        public Task<IUser[]> GetSubscribersAsync()
        {
            return Client.Lists.GetListSubscribersAsync(this);
        }

        public Task<ITwitterList> SubscribeAsync()
        {
            return Client.Lists.SubscribeToListAsync(this);
        }

        public Task<ITwitterList> UnsubscribeAsync()
        {
            return Client.Lists.UnsubscribeFromListAsync(this);
        }

        public Task<bool> CheckUserSubscriptionAsync(long userId)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfListAsync(this, userId);
        }

        public Task<bool> CheckUserSubscriptionAsync(string username)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfListAsync(this, username);
        }

        public Task<bool> CheckUserSubscriptionAsync(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfListAsync(this, user);
        }

        public async Task UpdateAsync(IListMetadataParameters parameters)
        {
            var updateListParams = new UpdateListParameters(this)
            {
                Name = parameters?.Name,
                Description = parameters?.Description,
                PrivacyMode = parameters?.PrivacyMode
            };

            var updateList = await Client.Lists.UpdateListAsync(updateListParams).ConfigureAwait(false);

            if (updateList != null)
            {
                _twitterListDTO = updateList.TwitterListDTO;
            }
        }

        public Task DestroyAsync()
        {
            return Client.Lists.DestroyListAsync(this);
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