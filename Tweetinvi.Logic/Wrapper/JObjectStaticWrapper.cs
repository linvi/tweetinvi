using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.Wrapper
{
    // Wrapper classes "cannot" be tested
    public class JObjectStaticWrapper : IJObjectStaticWrapper
    {
        private readonly JsonSerializer _serializer;

        public JObjectStaticWrapper()
        {
            _serializer = new JsonSerializer();
            
            foreach (var converter in JsonPropertiesConverterRepository.Converters)
            {
                _serializer.Converters.Add(converter);
            }
        }

        public JObject GetJobjectFromJson(string json)
        {
            if (!StringExtension.IsMatchingJsonFormat(json))
            {
                return null;
            }

            return JObject.Parse(json);
        }

        public T ToObject<T>(JObject jObject)
        {
            return jObject.ToObject<T>(_serializer);
        }

        public T ToObject<T>(JToken jToken) where T : class 
        {
            if (jToken == null)
            {
                return null;
            }

            return jToken.ToObject<T>(_serializer);
        }

        public string GetNodeRootName(JToken jToken)
        {
            var jProperty = jToken as JProperty;
            return jProperty != null ? jProperty.Name : null;
        }
    }
}