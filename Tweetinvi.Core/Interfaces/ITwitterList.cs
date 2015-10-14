using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITwitterList : ITwitterListAsync, ITwitterListIdentifier
    {
        ITwitterListDTO TwitterListDTO { get; set; }

        string IdStr { get; }
        string Name { get; }
        string FullName { get; }

        IUser Owner { get; }
        DateTime CreatedAt { get; }
        string Uri { get; }
        string Description { get; }
        bool Following { get; }
        PrivacyMode PrivacyMode { get; }

        int MemberCount { get; }
        int SubscriberCount { get; }

        IEnumerable<ITweet> GetTweets(IGetTweetsFromListParameters getTweetsFromListParameters = null);

        IEnumerable<IUser> GetMembers(int maximumNumberOfUsersToRetrieve = 100);

        bool AddMember(long userId);
        bool AddMember(string userScreenName);
        bool AddMember(IUserIdentifier user);

        MultiRequestsResult AddMultipleMembers(IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembers(IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembers(IEnumerable<IUserIdentifier> users);

        bool RemoveMember(long userId);
        bool RemoveMember(string userScreenName);
        bool RemoveMember(IUserIdentifier user);

        MultiRequestsResult RemoveMultipleMembers(IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembers(IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembers(IEnumerable<IUserIdentifier> users);

        bool CheckUserMembership(long userId);
        bool CheckUserMembership(string userScreenName);
        bool CheckUserMembership(IUserIdentifier user);

        IEnumerable<IUser> GetSubscribers(int maximumNumberOfUsersToRetrieve = 100);

        bool SubscribeLoggedUserToList(ILoggedUser loggedUser = null);
        bool UnSubscribeLoggedUserFromList(ILoggedUser loggedUser = null);

        bool CheckUserSubscription(long userId);
        bool CheckUserSubscription(string userScreenName);
        bool CheckUserSubscription(IUserIdentifier user);

        bool Update(ITwitterListUpdateParameters parameters);
        bool Destroy();
    }
}