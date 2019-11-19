using System;
using Newtonsoft.Json;

namespace Tweetinvi.Core.JsonConverters
{
    public class JsonUploadMetadataAltTextConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var altText = (value as string) ?? "";
            var obj = new
            {
                text = altText
            };

            serializer.Serialize(writer, obj);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}