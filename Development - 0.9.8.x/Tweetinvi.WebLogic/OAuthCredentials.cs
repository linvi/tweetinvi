using System.Runtime.Serialization;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    /// <summary>
    /// This class provides host basic information for authorizing a OAuth
    /// consumer to connect to a service. It does not contain any logic
    /// </summary>
    [DataContract]
    public class OAuthCredentials : IOAuthCredentials
    {
        [DataMember]
        public virtual string AccessToken { get; set; }

        [DataMember]
        public virtual string AccessTokenSecret { get; set; }

        [DataMember]
        public virtual string ConsumerKey { get; set; }

        [DataMember]
        public virtual string ConsumerSecret { get; set; }
    }
}