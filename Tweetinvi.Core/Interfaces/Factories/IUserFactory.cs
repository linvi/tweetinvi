using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IUserFactory
    {
        IAuthenticatedUser GetAuthenticatedUser(ITwitterCredentials credentials = null, IGetAuthenticatedUserParameters parameters = null);

        IUser GetUserFromId(long userId);
        IUser GetUserFromScreenName(string userName);

        // Generate User from Json
        IUser GenerateUserFromJson(string jsonUser);

        // Get Multiple users
        IEnumerable<IUser> GetUsersFromIds(IEnumerable<long> userIds);
        IEnumerable<IUser> GetUsersFromScreenNames(IEnumerable<string> userNames);

        // Generate user from DTO
        IUser GenerateUserFromDTO(IUserDTO userDTO);
        IAuthenticatedUser GenerateAuthenticatedUserFromDTO(IUserDTO userDTO);
        IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO);

        // Generate userIdentifier from
        IUserIdentifier GenerateUserIdentifierFromId(long userId);
        IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName);
    }
}