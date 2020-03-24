using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Models
{
    public enum EventType
    {
        /// <summary>
        /// Default value used when the string from Twitter is not a value in the Enum
        /// </summary>
        UnrecognisedValue = 0,

        [JsonEnumString("message_create")]
        MessageCreate
    }
}
