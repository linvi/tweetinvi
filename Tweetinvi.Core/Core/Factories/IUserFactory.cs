using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface IUserFactory
    {
        // Generate user from DTO
        IUser[] GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO, ITwitterClient client);
    }
}