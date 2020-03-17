using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Json;

namespace Tweetinvi.Client
{
    public class JsonClient : IJsonClient
    {
        private readonly ITweetinviJsonConverter _tweetinviJsonConverter;

        public JsonClient(ITweetinviJsonConverter tweetinviJsonConverter)
        {
            _tweetinviJsonConverter = tweetinviJsonConverter;
        }

        public string Serialize<T>(T obj) where T : class
        {
            return _tweetinviJsonConverter.ToJson(obj);
        }

        public T Deserialize<T>(string json) where T : class
        {
            return _tweetinviJsonConverter.ConvertJsonTo<T>(json);
        }
    }
}