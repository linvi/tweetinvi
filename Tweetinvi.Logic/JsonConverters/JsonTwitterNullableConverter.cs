using System;
using Newtonsoft.Json;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonTwitterNullableConverter<T> : JsonConverter where T : struct
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var nullableStruct = serializer.Deserialize<T?>(reader);
            if (nullableStruct == null)
            {
                nullableStruct = default (T);
            }

            return nullableStruct;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (T?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}