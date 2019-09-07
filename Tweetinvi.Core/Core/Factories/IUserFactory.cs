using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface IUserFactory
    {
        // Generate User from Json
        IUser GenerateUserFromJson(string jsonUser);

        // Get Multiple users
        Task<IEnumerable<IUser>> GetUsersFromIds(IEnumerable<long> userIds);

        // Generate user from DTO
        IUser GenerateUserFromDTO(IUserDTO userDTO);
        IAuthenticatedUser GenerateAuthenticatedUserFromDTO(IUserDTO userDTO, ITwitterCredentials credentials = null);
        IUser[] GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO);

        // Generate userIdentifier from
        IUserIdentifier GenerateUserIdentifierFromId(long userId);
        IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName);
    }
}