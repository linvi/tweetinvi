using Newtonsoft.Json;

namespace Tweetinvi.Core.Wrappers
{
    public interface IJsonConvertWrapper
    {
        T DeserializeObject<T>(string json);
        T DeserializeObject<T>(string json, JsonConverter[] converters);
    }
}