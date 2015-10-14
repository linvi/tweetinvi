using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonObjectConverter : IJsonObjectConverter
    {
        private readonly IJsonConvertWrapper _jsonConvertWrapper;

        public JsonObjectConverter(IJsonConvertWrapper jsonConvertWrapper)
        {
            _jsonConvertWrapper = jsonConvertWrapper;
        }

        public T DeserializeObject<T>(string json, JsonConverter[] converters = null) where T : class
        {
            if (!StringExtension.IsMatchingJsonFormat(json))
            {
                return default(T);
            }

            if (converters == null)
            {
                return _jsonConvertWrapper.DeserializeObject<T>(json, JsonPropertiesConverterRepository.Converters);
            }

            return _jsonConvertWrapper.DeserializeObject<T>(json, converters);
        }
    }
}