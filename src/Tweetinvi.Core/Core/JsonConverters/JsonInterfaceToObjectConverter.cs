using System;
using Newtonsoft.Json;

namespace Tweetinvi.Core.JsonConverters
{
    public interface IJsonInterfaceToObjectConverter
    {
        Type InterfaceType { get; }
    }

    public class JsonInterfaceToObjectConverter<TInterface, TTo> : JsonConverter, IJsonInterfaceToObjectConverter
        where TTo : TInterface
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<TTo>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType == typeof (TInterface);
            return canConvert;
        }

        public Type InterfaceType => typeof (TInterface);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}