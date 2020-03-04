using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.DTO.Cursor
{
    public class MessageCursorQueryResultDTO : BaseCursorQueryDTO<IMessageEventDTO>, IMessageCursorQueryResultDTO
    {
        private IMessageEventDTO[] _messageEvents;

        // This property does not exists
        [JsonIgnore]
        public override long NextCursor { get; set; }

        [JsonProperty("next_cursor")]
        public override string NextCursorStr { get; set; }

        public override int GetNumberOfObjectRetrieved()
        {
            return MessageEvents.Length;
        }

        [JsonProperty("events")]
        public IMessageEventDTO[] MessageEvents
        {
            get => _messageEvents ?? new IMessageEventDTO[0];
            set
            {
                _messageEvents = value;
                Results = value;
            }
        }

        [JsonProperty("apps")]
        public Dictionary<long, IApp> Apps { get; set; }
    }
}
