using Newtonsoft.Json;

namespace Tweetinvi.Core.Helpers
{
    /// <summary>
    /// This interface allows to (de)serialize any object or interface from the Tweetinvi API
    /// </summary>
    public interface IJsonObjectConverter
    {
        string SerializeObject(object o, JsonConverter[] converters = null);
        T DeserializeObject<T>(string json, JsonConverter[] converters = null) where T : class;
    }
}