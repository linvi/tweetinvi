using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IListsClient
    {
        /// <inheritdoc cref="CreateList(ICreateListParameters)"/>
        Task<ITwitterList> CreateList(string name);

        /// <inheritdoc cref="CreateList(ICreateListParameters)"/>
        Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode);

        /// <summary>
        /// Create a list on Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create </para>
        /// <returns>Created list</returns>
        Task<ITwitterList> CreateList(ICreateListParameters parameters);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(long listId);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(string slug, IUserIdentifier user);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(ITwitterListIdentifier listId);

        /// <summary>
        /// Get a specific list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-show </para>
        /// <returns>List requested</returns>
        Task<ITwitterList> GetList(IGetListParameters parameters);

        /// <inheritdoc cref="GetListsSubscribedByAccount(IGetListsSubscribedByAccountParameters)"/>
        Task<ITwitterList[]> GetListsSubscribedByAccount();

        /// <summary>
        /// Get lists subscribed by the current account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>Account user's lists</returns>
        Task<ITwitterList[]> GetListsSubscribedByAccount(IGetListsSubscribedByAccountParameters parameters);

        /// <inheritdoc cref="GetListsSubscribedByUser(IGetListsSubscribedByUserParameters)"/>
        Task<ITwitterList[]> GetListsSubscribedByUser(long userId);
        /// <inheritdoc cref="GetListsSubscribedByUser(IGetListsSubscribedByUserParameters)"/>
        Task<ITwitterList[]> GetListsSubscribedByUser(string username);
        /// <inheritdoc cref="GetListsSubscribedByUser(IGetListsSubscribedByUserParameters)"/>
        Task<ITwitterList[]> GetListsSubscribedByUser(IUserIdentifier user);

        /// <summary>
        /// Get lists subscribed by a specific user
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>User's lists</returns>
        Task<ITwitterList[]> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters);

        /// <summary>
        /// Update information of a Twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-update </para>
        /// <returns>Updated list</returns>
        Task<ITwitterList> UpdateList(IUpdateListParameters parameters);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(long listId);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(string slug, IUserIdentifier user);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(ITwitterListIdentifier listId);

        /// <summary>
        /// Destroy a list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-destroy </para>
        Task DestroyList(IDestroyListParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByAccount(IGetListsOwnedByAccountParameters)"/>
        Task<ITwitterList[]> GetListsOwnedByAccount();

        /// <summary>
        /// Get the lists owned by the account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>Lists owned by the account</returns>
        Task<ITwitterList[]> GetListsOwnedByAccount(IGetListsOwnedByAccountParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator();

        /// <summary>
        /// Get the lists owned by the account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>An iterator over the lists owned by the account</returns>
        ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByUser(IGetListsOwnedByUserParameters)"/>
        Task<ITwitterList[]> GetListsOwnedByUser(long userId);
        /// <inheritdoc cref="GetListsOwnedByUser(IGetListsOwnedByUserParameters)"/>
        Task<ITwitterList[]> GetListsOwnedByUser(string username);
        /// <inheritdoc cref="GetListsOwnedByUser(IGetListsOwnedByUserParameters)"/>
        Task<ITwitterList[]> GetListsOwnedByUser(IUser user);

        /// <summary>
        /// Get the lists owned by a user
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>Lists owned by a user</returns>
        Task<ITwitterList[]> GetListsOwnedByUser(IGetListsOwnedByUserParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(long userId);
        /// <inheritdoc cref="GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(string username);
        /// <inheritdoc cref="GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(IUser user);

        /// <summary>
        /// Get the lists owned by a user
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>An iterator over the lists owned by the user</returns>
        ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters);

        // ***********
        // MEMBERSHIP
        // ***********

        /// <inheritdoc cref="AddMemberToList(IAddMemberToListParameters)"/>
        Task AddMemberToList(long listId, long userId);
        /// <inheritdoc cref="AddMemberToList(IAddMemberToListParameters)"/>
        Task AddMemberToList(ITwitterListIdentifier list, long userId);
        /// <inheritdoc cref="AddMemberToList(IAddMemberToListParameters)"/>
        Task AddMemberToList(ITwitterListIdentifier list, string username);
        /// <inheritdoc cref="AddMemberToList(IAddMemberToListParameters)"/>
        Task AddMemberToList(ITwitterListIdentifier list, IUserIdentifier user);

        /// <summary>
        /// Add a member to a twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create </para>
        Task AddMemberToList(IAddMemberToListParameters parameters);

        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(long listId, IEnumerable<long> userIds);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(long listId, IEnumerable<string> usernames);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(long listId, IEnumerable<IUserIdentifier> users);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<string> usernames);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Add multiple members to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create_all </para>
        /// </summary>
        Task AddMembersToList(IAddMembersToListParameters parameters);

        /// <inheritdoc cref="GetAccountListMemberships(IGetAccountListMembershipsParameters)"/>
        Task<ITwitterList[]> GetAccountListMemberships();

        /// <summary>
        /// Get the lists the account is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>Lists the account is member of</returns>
        Task<ITwitterList[]> GetAccountListMemberships(IGetAccountListMembershipsParameters parameters);

        /// <inheritdoc cref="GetAccountListMembershipsIterator(IGetAccountListMembershipsParameters)"/>
        ITwitterIterator<ITwitterList> GetAccountListMembershipsIterator();

        /// <summary>
        /// Get an iterator to retrieve all the lists the account is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>An iterator to retrieve all the lists the account is member of</returns>
        ITwitterIterator<ITwitterList> GetAccountListMembershipsIterator(IGetAccountListMembershipsParameters parameters);

        /// <inheritdoc cref="GetUserListMemberships(IGetUserListMembershipsParameters)"/>
        Task<ITwitterList[]> GetUserListMemberships(long userId);
        /// <inheritdoc cref="GetUserListMemberships(IGetUserListMembershipsParameters)"/>
        Task<ITwitterList[]> GetUserListMemberships(string username);
        /// <inheritdoc cref="GetUserListMemberships(IGetUserListMembershipsParameters)"/>
        Task<ITwitterList[]> GetUserListMemberships(IUserIdentifier user);

        /// <summary>
        /// Get an iterator to retrieve all the lists a user is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>Lists a user is member of</returns>
        Task<ITwitterList[]> GetUserListMemberships(IGetUserListMembershipsParameters parameters);

        /// <inheritdoc cref="GetUserListMembershipsIterator(IGetUserListMembershipsParameters)"/>
        ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(long userId);
        /// <inheritdoc cref="GetUserListMembershipsIterator(IGetUserListMembershipsParameters)"/>
        ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(string username);
        /// <inheritdoc cref="GetUserListMembershipsIterator(IGetUserListMembershipsParameters)"/>
        ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(IUserIdentifier user);

        /// <summary>
        /// Get an iterator to retrieve all the lists a user is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>An iterator to retrieve all the lists a user is member of</returns>
        ITwitterIterator<ITwitterList> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters);

        /// <inheritdoc cref="GetMembersOfList(IGetMembersOfListParameters)"/>
        Task<IUser[]> GetMembersOfList(long listId);
        /// <inheritdoc cref="GetMembersOfList(IGetMembersOfListParameters)"/>
        Task<IUser[]> GetMembersOfList(ITwitterListIdentifier list);

        /// <summary>
        /// Get the members of the specified list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members </para>
        /// </summary>
        /// <returns>Members of the list</returns>
        Task<IUser[]> GetMembersOfList(IGetMembersOfListParameters parameters);

        /// <inheritdoc cref="GetMembersOfListIterator(IGetMembersOfListParameters)"/>
        ITwitterIterator<IUser> GetMembersOfListIterator(long listId);
        /// <inheritdoc cref="GetMembersOfListIterator(IGetMembersOfListParameters)"/>
        ITwitterIterator<IUser> GetMembersOfListIterator(ITwitterListIdentifier list);

        /// <summary>
        /// Get the members of the specified list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members </para>
        /// </summary>
        /// <returns>An iterator to list the users members of the list</returns>
        ITwitterIterator<IUser> GetMembersOfListIterator(IGetMembersOfListParameters parameters);

        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long listId, long userId);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long listId, string username);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long listId, IUserIdentifier user);

        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, long userId);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, string username);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, IUserIdentifier user);

        /// <summary>
        /// Check if a user is a member of a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members-show </para>
        /// </summary>
        /// <returns>Returns whether the user is a member of a list</returns>
        Task<bool> CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters parameters);

        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(long listId, long userId);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(long listId, string username);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(long listId, IUserIdentifier user);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(ITwitterListIdentifier list, long userId);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(ITwitterListIdentifier list, string username);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user);

        /// <summary>
        /// Remove a member from a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy </para>
        /// </summary>
        Task RemoveMemberFromList(IRemoveMemberFromListParameters parameters);

        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(long listId, IEnumerable<long> userIds);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(long listId, IEnumerable<string> usernames);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(long listId, IEnumerable<IUserIdentifier> users);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<string> usernames);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Remove multiple members from a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy_all </para>
        /// </summary>
        Task RemoveMembersFromList(IRemoveMembersFromListParameters parameters);

        // ***********
        // SUBSCRIBERS
        // ***********

        /// <inheritdoc cref="SubscribeToList(ITwitterListIdentifier)" />
        Task<ITwitterList> SubscribeToList(long listId);
        /// <inheritdoc cref="SubscribeToList(ITwitterListIdentifier)" />
        Task<ITwitterList> SubscribeToList(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated account to the specified list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-create </para>
        /// </summary>
        /// <returns>The latest version of the list</returns>
        Task<ITwitterList> SubscribeToList(ISubscribeToListParameters parameters);

        /// <inheritdoc cref="UnsubscribeFromList(IUnsubscribeFromListParameters)" />
        Task<ITwitterList> UnsubscribeFromList(long listId);
        /// <inheritdoc cref="UnsubscribeFromList(IUnsubscribeFromListParameters)" />
        Task<ITwitterList> UnsubscribeFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Unsubscribe the authenticated account from the specified list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-destroy </para>
        /// </summary>
        /// <returns>The latest version of the list</returns>
        Task<ITwitterList> UnsubscribeFromList(IUnsubscribeFromListParameters parameters);

        /// <inheritdoc cref="GetListSubscribers(ITwitterListIdentifier)" />
        Task<IUser[]> GetListSubscribers(long listId);
        /// <inheritdoc cref="GetListSubscribers(ITwitterListIdentifier)" />
        Task<IUser[]> GetListSubscribers(ITwitterListIdentifier list);

        /// <summary>
        /// Returns the users subscribed to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers </para>
        /// </summary>
        /// <returns>Subscribers of the list</returns>
        Task<IUser[]> GetListSubscribers(IGetListSubscribersParameters parameters);

        /// <inheritdoc cref="GetListSubscribersIterator(ITwitterListIdentifier)" />
        ITwitterIterator<IUser> GetListSubscribersIterator(long listId);
        /// <inheritdoc cref="GetListSubscribersIterator(ITwitterListIdentifier)" />
        ITwitterIterator<IUser> GetListSubscribersIterator(ITwitterListIdentifier list);

        /// <summary>
        /// Returns the users subscribed to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers </para>
        /// </summary>
        /// <returns>An iterator to get through the subscribers of the list</returns>
        ITwitterIterator<IUser> GetListSubscribersIterator(IGetListSubscribersParameters parameters);

        /// <summary>
        /// Returns the lists the account subscribed to
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions </para>
        /// </summary>
        /// <returns>An iterator the get though the lists the account subscribed to</returns>
        ITwitterIterator<ITwitterList> GetAccountListSubscriptionsIterator(IGetAccountListSubscriptionsParameters parameters);

        /// <inheritdoc cref="GetUserListSubscriptions(IGetUserListSubscriptionsParameters)" />
        Task<ITwitterList[]> GetUserListSubscriptions(long userId);
        /// <inheritdoc cref="GetUserListSubscriptions(IGetUserListSubscriptionsParameters)" />
        Task<ITwitterList[]> GetUserListSubscriptions(string username);
        /// <inheritdoc cref="GetUserListSubscriptions(IGetUserListSubscriptionsParameters)" />
        Task<ITwitterList[]> GetUserListSubscriptions(IUserIdentifier user);

        /// <summary>
        /// Returns the lists a user subscribed to
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions </para>
        /// </summary>
        /// <returns>Lists a user subscribed to</returns>
        Task<ITwitterList[]> GetUserListSubscriptions(IGetUserListSubscriptionsParameters parameters);

        /// <inheritdoc cref="GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters)" />
        ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(long userId);
        /// <inheritdoc cref="GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters)" />
        ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(string username);
        /// <inheritdoc cref="GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters)" />
        ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(IUserIdentifier user);

        /// <summary>
        /// Returns the lists a user subscribed to
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions </para>
        /// </summary>
        /// <returns>An iterator the get though the lists a user subscribed to</returns>
        ITwitterIterator<ITwitterList> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters);

        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(long listId, long userId);
        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(long listId, string username);
        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(long listId, IUserIdentifier user);
        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, long userId);
        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, string username);
        /// <inheritdoc cref="CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters)" />
        Task<bool> CheckIfUserIsSubscriberOfList(ITwitterListIdentifier list, IUserIdentifier user);

        /// <summary>
        /// Check if a user is a subscriber of a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers-show </para>
        /// </summary>
        /// <returns>Whether the user is a subscriber of the list</returns>
        Task<bool> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters);

        // ***********
        // TWEETS
        // ***********

        /// <inheritdoc cref="GetTweetsFromList(IGetTweetsFromListParameters)" />
        Task<ITweet[]> GetTweetsFromList(long listId);
        /// <inheritdoc cref="GetTweetsFromList(IGetTweetsFromListParameters)" />
        Task<ITweet[]> GetTweetsFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Returns the tweets authored by the members of the list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses </para>
        /// </summary>
        /// <returns>Tweets of a list</returns>
        Task<ITweet[]> GetTweetsFromList(IGetTweetsFromListParameters parameters);

        /// <inheritdoc cref="GetTweetsFromListIterator(IGetTweetsFromListParameters)" />
        ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(long listId);
        /// <inheritdoc cref="GetTweetsFromListIterator(IGetTweetsFromListParameters)" />
        ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(ITwitterListIdentifier list);

        /// <summary>
        /// Returns the tweets authored by the members of the list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses </para>
        /// </summary>
        /// <returns>An iterator to get through the tweets of a list</returns>
        ITwitterIterator<ITweet, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters);
    }
}