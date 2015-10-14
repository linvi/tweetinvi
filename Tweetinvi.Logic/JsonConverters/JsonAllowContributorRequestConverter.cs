using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonAllowContributorRequestConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "none" :
                        return AllowContributorRequestMode.None;
                    case "following":
                        return AllowContributorRequestMode.Followers;
                    case "all" :
                        return AllowContributorRequestMode.All;
                }
            }

            return AllowContributorRequestMode.None;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AllowContributorRequestMode);
        }
    }
}