using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.User
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserFactoryQueryExecutor _userFactoryQueryExecutor;
        private readonly IFactory<IAuthenticatedUser> _authenticatedUserUnityFactory;
        private readonly IFactory<IUser> _userUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public UserFactory(
            IUserFactoryQueryExecutor userFactoryQueryExecutor,
            IFactory<IAuthenticatedUser> authenticatedUserUnityFactory,
            IFactory<IUser> userUnityFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _userFactoryQueryExecutor = userFactoryQueryExecutor;
            _authenticatedUserUnityFactory = authenticatedUserUnityFactory;
            _userUnityFactory = userUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Generate User from Json
        public IUser GenerateUserFromJson(string jsonUser)
        {
            var userDTO = _jsonObjectConverter.DeserializeObject<IUserDTO>(jsonUser);
            return GenerateUserFromDTO(userDTO, null);
        }

        public async Task<IEnumerable<IUser>> GetUsersFromIds(IEnumerable<long> userIds)
        {
            var usersDTO = await _userFactoryQueryExecutor.GetUsersDTOFromIds(userIds);
            return GenerateUsersFromDTO(usersDTO, null);
        }

        // Generate DTO from id
        public IUserIdentifier GenerateUserIdentifierFromId(long userId)
        {
            return new UserIdentifier(userId);
        }

        public IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName)
        {
            return new UserIdentifier(userScreenName);
        }

        // Generate from DTO
        public IAuthenticatedUser GenerateAuthenticatedUserFromDTO(IUserDTO userDTO, ITwitterCredentials credentials = null)
        {
            if (userDTO == null)
            {
                return null;
            }

            var userDTOParameterOverride = _authenticatedUserUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var user = _authenticatedUserUnityFactory.Create(userDTOParameterOverride);

            if (credentials != null)
            {
                user.Credentials = credentials;
            }

            return user;
        }

        public IUser GenerateUserFromDTO(IUserDTO userDTO, ITwitterClient client)
        {
            if (userDTO == null)
            {
                return null;
            }

            var userDTOParameterOverride = _userUnityFactory.GenerateParameterOverrideWrapper("userDTO", userDTO);
            var clientParameterOverride = _userUnityFactory.GenerateParameterOverrideWrapper("client", null);

            var user = _userUnityFactory.Create(userDTOParameterOverride, clientParameterOverride);

            user.Client = client;

            return user;
        }

        public IUser[] GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO, ITwitterClient client)
        {
            if (usersDTO == null)
            {
                return null;
            }

            return usersDTO.Select(x => GenerateUserFromDTO(x, client)).ToArray();
        }
    }
}