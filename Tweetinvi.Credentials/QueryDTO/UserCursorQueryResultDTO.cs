using Newtonsoft.Json;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Credentials.QueryDTO
{
    public class UserCursorQueryResultDTO : BaseCursorQueryDTO<IUserDTO>, IUserCursorQueryResultDTO
    {
        private IUserDTO[] _users;

        [JsonProperty("users")]
        public IUserDTO[] Users
        {
            get { return _users ?? new IUserDTO[0]; }
            set
            {
                _users = value;
                Results = value;
            }
        }

        public override int GetNumberOfObjectRetrieved()
        {
            return Users.Length;
        }
    }
}