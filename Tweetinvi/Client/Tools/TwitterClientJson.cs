using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Client.Tools
{
    public class TwitterClientJson : ITwitterClientJson
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public TwitterClientJson(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public T DeserializeObject<T>(string json)
        {
            return _jsonObjectConverter.DeserializeObject<T>(json);
        }
    }
}