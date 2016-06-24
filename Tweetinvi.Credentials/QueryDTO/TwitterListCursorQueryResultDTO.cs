using Newtonsoft.Json;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

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