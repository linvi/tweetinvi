using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonLanguageConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return Language.English;
            }
            
            int parsed;

            if (int.TryParse(reader.Value.ToString(), out parsed))
            {
                return LanguageExtension.GetLangFromDescription(parsed);
            }

            return LanguageExtension.GetLangFromDescription((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Language);
        }
    }
}
