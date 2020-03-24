using Newtonsoft.Json;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.DTO.Cursor
{
    public class TwitterListCursorQueryResultDTO : BaseCursorQueryDTO<ITwitterListDTO>, ITwitterListCursorQueryResultDTO
    {
        private ITwitterListDTO[] _twitterLists;

        [JsonProperty("lists")]
        public ITwitterListDTO[] TwitterLists
        {
            get => _twitterLists ?? new ITwitterListDTO[0];
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