using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Core.Web
{
    public class JsonContentFactory
    {
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public JsonContentFactory(IJsonObjectConverter jsonObjectConverter)
        {
            _jsonObjectConverter = jsonObjectConverter;
        }

        public StringContent Create<T>(T content)
        {
            return Create(content, null);
        }

        public StringContent Create<T>(T content, JsonConverter[] converters)
        {
            var jsonBody = _jsonObjectConverter.Serialize(content, converters);
            return new StringContent(jsonBody, Encoding.UTF8, "application/json");
        }
    }
}