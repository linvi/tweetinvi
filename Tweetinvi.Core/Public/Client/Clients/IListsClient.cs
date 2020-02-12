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
        Task<ITwitterList> GetList(long? listId);

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
        Task<ITwitterList[]> GetListsSubscribedByUser(long? userId);
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
        Task DestroyList(long? listId);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(string slug, IUserIdentifier user);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(ITwitterListIdentifier listId);

        /// <summary>
        /// Destroy a list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-destroy </para>
        Task DestroyList(IDestroyListParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator();

        /// <summary>
        /// Get the lists owned by the account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>An iterator over the lists owned by the account</returns>
        ITwitterIterator<ITwitterList> GetListsOwnedByAccountIterator(IGetListsOwnedByAccountParameters parameters);

        /// <inheritdoc cref="GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters)"/>
        ITwitterIterator<ITwitterList> GetListsOwnedByUserIterator(long? userId);
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
        Task AddMemberToList(long? listId, long? userId);
        /// <inheritdoc cref="AddMemberToList(IAddMemberToListParameters)"/>
        Task AddMemberToList(ITwitterListIdentifier list, long? userId);
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
        Task AddMembersToList(long? listId, IEnumerable<long?> userIds);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(long? listId, IEnumerable<string> usernames);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(long? listId, IEnumerable<IUserIdentifier> users);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<long?> userIds);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<string> usernames);
        /// <inheritdoc cref="AddMembersToList(IAddMembersToListParameters)"/>
        Task AddMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Add multiple members to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create_all </para>
        /// </summary>
        Task AddMembersToList(IAddMembersToListParameters parameters);

        /// <inheritdoc cref="GetListsAccountIsMemberOfIterator(IGetListsAccountIsMemberOfParameters)"/>
        ITwitterIterator<ITwitterList> GetListsAccountIsMemberOfIterator();

        /// <summary>
        /// Get an iterator to retrieve all the lists the account is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>An iterator to retrieve all the lists the account is member of</returns>
        ITwitterIterator<ITwitterList> GetListsAccountIsMemberOfIterator(IGetListsAccountIsMemberOfParameters parameters);

        /// <inheritdoc cref="GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters)"/>
        ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(long? userId);
        /// <inheritdoc cref="GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters)"/>
        ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(string username);
        /// <inheritdoc cref="GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters)"/>
        ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(IUserIdentifier user);

        /// <summary>
        /// Get an iterator to retrieve all the lists a user is member of
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// </summary>
        /// <returns>An iterator to retrieve all the lists a user is member of</returns>
        ITwitterIterator<ITwitterList> GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters parameters);

        /// <inheritdoc cref="GetMembersOfListIterator(IGetMembersOfListParameters)"/>
        ITwitterIterator<IUser> GetMembersOfListIterator(long? listId);
        /// <inheritdoc cref="GetMembersOfListIterator(IGetMembersOfListParameters)"/>
        ITwitterIterator<IUser> GetMembersOfListIterator(ITwitterListIdentifier list);

        /// <summary>
        /// Get the members of the specified list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members </para>
        /// </summary>
        /// <returns>An iterator to list the users members of the list</returns>
        ITwitterIterator<IUser> GetMembersOfListIterator(IGetMembersOfListParameters parameters);

        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long? listId, long? userId);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long? listId, string username);
        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(long? listId, IUserIdentifier user);

        /// <inheritdoc cref="CheckIfUserIsMemberOfList(ICheckIfUserIsMemberOfListParameters)"/>
        Task<bool> CheckIfUserIsMemberOfList(ITwitterListIdentifier list, long? userId);
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
        Task RemoveMemberFromList(long? listId, long? userId);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(long? listId, string username);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(long? listId, IUserIdentifier user);
        /// <inheritdoc cref="RemoveMemberFromList(IRemoveMemberFromListParameters)"/>
        Task RemoveMemberFromList(ITwitterListIdentifier list, long? userId);
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
        Task RemoveMembersFromList(long? listId, IEnumerable<long?> userIds);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(long? listId, IEnumerable<string> usernames);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(long? listId, IEnumerable<IUserIdentifier> users);
        /// <inheritdoc cref="RemoveMembersFromList(IRemoveMembersFromListParameters)"/>
        Task RemoveMembersFromList(ITwitterListIdentifier list, IEnumerable<long?> userIds);
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
        // GET TWEETS
        // ***********

        /// <inheritdoc cref="GetTweetsFromList(IGetTweetsFromListParameters)" />
        ITwitterIterator<ITweet, long?> GetTweetsFromList(long? listId);
        /// <inheritdoc cref="GetTweetsFromList(IGetTweetsFromListParameters)" />
        ITwitterIterator<ITweet, long?> GetTweetsFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Returns the tweets authored by the members of the list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses </para>
        /// </summary>
        /// <returns>An iterator to get through the tweets of a list</returns>
        ITwitterIterator<ITweet, long?> GetTweetsFromList(IGetTweetsFromListParameters parameters);
    }
}