using System.Runtime.Serialization;

namespace Tweetinvi.Models
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
        PUT,
        [EnumMember]
        DELETE
    }
}