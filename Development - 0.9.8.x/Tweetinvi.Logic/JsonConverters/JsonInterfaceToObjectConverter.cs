using System;
using Newtonsoft.Json;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonInterfaceToObjectConverter<T, U> : JsonConverter 
        where U : T
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<U>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (T));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}