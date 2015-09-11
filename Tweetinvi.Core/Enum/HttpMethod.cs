using System.Runtime.Serialization;

namespace Tweetinvi.Core.Enum
{
    /// <summary>
    /// Enumeration of possible HTTP request method
    /// </summary>
    [DataContract]
    public enum HttpMethod
    {
        [EnumMember]
        GET,
        [EnumMember]
        POST,
        [EnumMember]
        DELETE
    }
}