using System;
using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Core.DTO
{
    public class UserIdentifierDTO : IUserIdentifier
    {
        private long? _id;

        [JsonProperty("id")]
        public long? Id
        {
            get
            {
                if (_id == null)
                {
                    _id = IdStr == null ? null : (long?)Int64.Parse(IdStr);
                }

                return _id;
            }
            set
            {
                _id = value;
                IdStr = _id.ToString();
            }
        }

        [JsonProperty("id_str")]
        public string IdStr { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }
    }
}