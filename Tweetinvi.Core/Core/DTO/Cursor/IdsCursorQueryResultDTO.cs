using Newtonsoft.Json;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.DTO.Cursor
{
    public class IdsCursorQueryResultDTO : BaseCursorQueryDTO<long>, IIdsCursorQueryResultDTO
    {
        private long[] _ids;

        [JsonProperty("ids")]
        public long[] Ids
        {
            get => _ids ?? new long[0];
            set
            {
                _ids = value;
                Results = value;
            }
        }

        public override int GetNumberOfObjectRetrieved()
        {
            return Ids.Length;
        }
    }
}