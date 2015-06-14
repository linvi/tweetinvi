using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;

namespace Tweetinvi.Credentials.QueryDTO
{
    public class TwitterListCursorQueryResultDTO : BaseCursorQueryDTO<ITwitterListDTO>, ITwitterListCursorQueryResultDTO
    {
        private ITwitterListDTO[] _twitterLists;

        [JsonProperty("lists")]
        public ITwitterListDTO[] TwitterLists
        {
            get { return _twitterLists ?? new ITwitterListDTO[0]; }
            set
            {
                _twitterLists = value;
                Results = value;
            }
        }

        public override int GetNumberOfObjectRetrieved()
        {
            return TwitterLists.Length;
        }
    }
}