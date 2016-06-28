using System;
using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.DTO
{
    public class UserIdentifierDTO : IUserIdentifier
    {
        private long? _id;
        public long Id
        {
            get
            {
                if (_id == null)
                {
                    _id = IdStr == null ? TweetinviSettings.DEFAULT_ID : Int64.Parse(IdStr);
                }

                return _id.Value;
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