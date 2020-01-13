using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.User
{
    public class UserFactory : IUserFactory
    {
        private readonly ITwitterClientFactories _factories;

        public UserFactory(ITwitterClientFactories factories)
        {
            _factories = factories;
        }

        public IUser[] GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO, ITwitterClient client)
        {
            return usersDTO?.Select(x => _factories.CreateUser(x)).ToArray();
        }
    }
}