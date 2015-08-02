using System;
using Newtonsoft.Json;

namespace Tweetinvi.Core.Helpers
{
    public interface IJsonPropertyConverterRepository
    {
        JsonConverter GetObjectConverter(object objectToConvert);
        JsonConverter GetTypeConverter(Type objectType);

        bool CanConvert(Type objectType);
        object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);
        void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);
    }
}