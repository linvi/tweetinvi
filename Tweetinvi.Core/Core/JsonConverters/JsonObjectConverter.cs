using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Core.JsonConverters
{
    public class JsonObjectConverter : IJsonObjectConverter
    {
        private readonly IJsonConvertWrapper _jsonConvertWrapper;

        public JsonObjectConverter(IJsonConvertWrapper jsonConvertWrapper)
        {
            _jsonConvertWrapper = jsonConvertWrapper;
        }

        public string Serialize(object o, JsonConverter[] converters = null)
        {
            if (converters == null)
            {
                converters = JsonPropertiesConverterRepository.Converters;
            }

            return _jsonConvertWrapper.SerializeObject(o, converters);
        }

        public T Deserialize<T>(string json, JsonConverter[] converters = null)
        {
            if (!json.IsMatchingJsonFormat())
            {
                return default;
            }

            if (converters == null)
            {
                converters = JsonPropertiesConverterRepository.Converters;
            }

            return _jsonConvertWrapper.DeserializeObject<T>(json, converters);
        }
    }
}