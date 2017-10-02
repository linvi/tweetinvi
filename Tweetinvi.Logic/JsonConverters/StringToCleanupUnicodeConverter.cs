using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Core.Helpers;

namespace Tweetinvi.Logic.JsonConverters
{
    public class StringToCleanupUnicodeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var altText = reader.Value as string ?? "";
            var cleanString = UnicodeHelper.UnicodeCleanup(altText);

            return cleanString;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
