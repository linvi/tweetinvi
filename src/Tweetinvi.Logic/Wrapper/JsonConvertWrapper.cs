using Newtonsoft.Json;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Logic.Wrapper
{
    // Wrapper classes "cannot" be tested
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public string SerializeObject(object o, JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(o, converters);
        }

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