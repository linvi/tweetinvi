using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.Json
{
    public class JsonClientTests : TweetinviTest
    {
        public JsonClientTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task Users()
        {
            var user = await _tweetinviClient.Users.GetAuthenticatedUser();
            var userJson = _tweetinviClient.Json.Serialize(user);
            var deserializedUser = _tweetinviClient.Json.Deserialize<IUser>(userJson);

            var userDtoJson = _tweetinviClient.Json.Serialize(user.UserDTO);
            var deserializedUserDto = _tweetinviClient.Json.Deserialize<IUserDTO>(userDtoJson);

            var users = new[] {user};

            var usersArrayJson = _tweetinviClient.Json.Serialize(users);
            var deserializedArrayUsers = _tweetinviClient.Json.Deserialize<IUser[]>(usersArrayJson);

            var usersListJson = _tweetinviClient.Json.Serialize(users.ToList());
            var deserializedListUsers = _tweetinviClient.Json.Deserialize<IUser[]>(usersListJson);

            var userDtos = new[] {user.UserDTO};

            var userDtosArrayJson = _tweetinviClient.Json.Serialize(userDtos);
            var deserializedArrayUserDtos = _tweetinviClient.Json.Deserialize<IUserDTO[]>(userDtosArrayJson);
            var deserializedArrayUserFromJsonDto = _tweetinviClient.Json.Deserialize<IUser[]>(userDtosArrayJson);

            var userDtosListJson = _tweetinviClient.Json.Serialize(userDtos.ToList());
            var deserializedListUserDtos = _tweetinviClient.Json.Deserialize<IUserDTO[]>(userDtosListJson);
            var deserializedListUserFromJsonDto = _tweetinviClient.Json.Deserialize<IUser[]>(userDtosListJson);

            Assert.Equal(deserializedUser.Id, user.Id);
            Assert.Equal(deserializedUserDto.Id, user.Id);

            Assert.Equal(deserializedArrayUsers[0].Id, user.Id);
            Assert.Equal(deserializedListUsers[0].Id, user.Id);

            Assert.Equal(deserializedArrayUserDtos[0].Id, user.Id);
            Assert.Equal(deserializedListUserDtos[0].Id, user.Id);
            Assert.Equal(deserializedArrayUserFromJsonDto[0].Id, user.Id);
            Assert.Equal(deserializedListUserFromJsonDto[0].Id, user.Id);
        }
    }
}