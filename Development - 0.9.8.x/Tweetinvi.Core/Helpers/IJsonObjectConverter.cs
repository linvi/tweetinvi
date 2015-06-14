using Newtonsoft.Json;

namespace Tweetinvi.Core.Helpers
{
    /// <summary>
    /// This interface allows to deserialize any object or interface from the Tweetinvi API
    /// </summary>
    public interface IJsonObjectConverter
    {
        T DeserializeObject<T>(string json, JsonConverter[] converters = null) where T : class;
    }
}