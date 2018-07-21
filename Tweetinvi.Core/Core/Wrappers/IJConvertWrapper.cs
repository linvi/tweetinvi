using Newtonsoft.Json;

namespace Tweetinvi.Core.Wrappers
{
    public interface IJsonConvertWrapper
    {
        string SerializeObject(object o);
        string SerializeObject(object o, JsonConverter[] converters);
        T DeserializeObject<T>(string json);
        T DeserializeObject<T>(string json, JsonConverter[] converters);
    }
}