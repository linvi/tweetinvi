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

        public Task<ITweet[]> GetTweets()
        {
            return Client.Lists.GetTweetsFromList(this);
        }

        // Members
        public Task<IUser[]> GetMembers()
        {
            return Client.Lists.GetMembersOfList(new GetMembersOfListParameters(this));
        }

        public Task AddMember(long userId)
        {
            return Client.Lists.AddMemberToList(this, userId);
        }

        public Task AddMember(string username)
        {
            return Client.Lists.AddMemberToList(this, username);
        }

        public Task AddMember(IUserIdentifier user)
        {
            return Client.Lists.AddMemberToList(this, user);
        }

        public Task AddMembers(IEnumerable<long> userIds)
        {
            return Client.Lists.AddMembersToList(this, userIds);
        }

        public Task AddMembers(IEnumerable<string> usernames)
        {
            return Client.Lists.AddMembersToList(this, usernames);
        }

        public Task AddMembers(IEnumerable<IUserIdentifier> users)
        {
            return Client.Lists.AddMembersToList(this, users);
        }


        public Task<bool> RemoveMember(long userId)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, userId);
        }

        public Task<bool> RemoveMember(string username)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, username);
        }

        public Task<bool> RemoveMember(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, user);
        }


        public Task RemoveMembers(IEnumerable<long> userIds)
        {
            return Client.Lists.RemoveMembersFromList(this, userIds);
        }

        public Task RemoveMembers(IEnumerable<string> usernames)
        {
            return Client.Lists.RemoveMembersFromList(this, usernames);
        }

        public Task RemoveMembers(IEnumerable<IUserIdentifier> users)
        {
            return Client.Lists.RemoveMembersFromList(this, users);
        }


        public Task<bool> CheckUserMembership(long userId)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, userId);
        }

        public Task<bool> CheckUserMembership(string userScreenName)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, userScreenName);
        }

        public Task<bool> CheckUserMembership(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsMemberOfList(this, user);
        }

        // Subscribers
        public Task<IUser[]> GetSubscribers()
        {
            return Client.Lists.GetListSubscribers(this);
        }

        public Task<ITwitterList> Subscribe()
        {
            return Client.Lists.SubscribeToList(this);
        }

        public Task<ITwitterList> Unsubscribe()
        {
            return Client.Lists.UnsubscribeFromList(this);
        }

        public Task<bool> CheckUserSubscription(long userId)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfList(this, userId);
        }

        public Task<bool> CheckUserSubscription(string username)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfList(this, username);
        }

        public Task<bool> CheckUserSubscription(IUserIdentifier user)
        {
            return Client.Lists.CheckIfUserIsSubscriberOfList(this, user);
        }

        public async Task Update(IListMetadataParameters parameters)
        {
            var updateListParams = new UpdateListParameters(this)
            {
                Name = parameters?.Name,
                Description = parameters?.Description,
                PrivacyMode = parameters?.PrivacyMode
            };

            var updateList = await Client.Lists.UpdateList(updateListParams).ConfigureAwait(false);

            if (updateList != null)
            {
                _twitterListDTO = updateList.TwitterListDTO;
            }
        }

        public Task Destroy()
        {
            return Client.Lists.DestroyList(this);
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