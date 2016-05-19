using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonLanguageConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int parsed = 0;
            if (int.TryParse(reader.Value.ToString(), out parsed))
                return LanguageExtension.GetLangFromDescription(parsed);
            else
                return LanguageExtension.GetLangFromDescription((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Language);
        }
    }
}