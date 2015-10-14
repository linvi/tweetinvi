using Newtonsoft.Json;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Logic.Wrapper
{
    // Wrapper classes "cannot" be tested
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public T DeserializeObject<T>(string json, JsonConverter[] converters)
        {   
            return JsonConvert.DeserializeObject<T>(json, converters);
        }
    }
}