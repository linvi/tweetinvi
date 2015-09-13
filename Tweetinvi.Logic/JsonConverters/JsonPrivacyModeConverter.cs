using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonPrivacyModeConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isPublic = false;
            
            // Used when the json property is 'mode'
            if (reader.ValueType == typeof (string))
            {
                isPublic = serializer.Deserialize<string>(reader) == "public";
            }

            // Used when the json property is 'protected'
            if (reader.ValueType == typeof (bool))
            {
                isPublic = !serializer.Deserialize<bool>(reader);
            }

            if (isPublic)
            {
                return PrivacyMode.Public;
            }

            return PrivacyMode.Private;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrivacyMode);
        }
    }
}