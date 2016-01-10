using System;
using Newtonsoft.Json;

namespace Tweetinvi.Logic.JsonConverters
{
    public interface IJsonInterfaceToObjectConverter
    {
        Type InterfaceType { get; }
    }

    public class JsonInterfaceToObjectConverter<T, U> : JsonConverter, IJsonInterfaceToObjectConverter
        where U : T
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<U>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            var canConvert = objectType == typeof (T);
            return canConvert;
        }

        public Type InterfaceType
        {
            get { return typeof (T); }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}