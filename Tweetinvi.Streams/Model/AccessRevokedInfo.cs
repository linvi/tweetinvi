using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO.StreamMessages;

namespace Tweetinvi.Streams.Model
{
    public class AccessRevokedInfo : IAccessRevokedInfo
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonIgnore]
        public long ApplicationId
        {
            get { return _accessRevokedClientApplication.Id; }
        }

        [JsonIgnore]
        public string ApplicationURL
        {
            get { return _accessRevokedClientApplication.URL; }
        }

        [JsonIgnore]
        public string ApplicationConsumerKey
        {
            get { return _accessRevokedClientApplication.ConsumerKey; }
        }

        [JsonIgnore]
        public string ApplicationName
        {
            get { return _accessRevokedClientApplication.Name; }
        }

        // ReSharper disable once UnassignedField.Compiler
#pragma warning disable 649
        [JsonProperty("client_application")]
        private AccessRevokedClientApplication _accessRevokedClientApplication;
#pragma warning restore 649

        // ReSharper disable once ClassNeverInstantiated.Local
        private class AccessRevokedClientApplication
        {
            [JsonProperty("id")]
            public long Id { get; private set; }

            [JsonProperty("url")]
            public string URL { get; private set; }

            [JsonProperty("consumer_key")]
            public string ConsumerKey { get; private set; }

            [JsonProperty("name")]
            public string Name { get; private set; }
        }
    }
}