using System;
using Newtonsoft.Json;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonAllowDirectMessagesConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                var value = serializer.Deserialize<string>(reader);
                switch (value)
                {
                    case "following":
                        return AllowDirectMessagesFrom.Following;
                    case "all":
                        return AllowDirectMessagesFrom.All;
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
            return objectType == typeof(AllowDirectMessagesFrom);
        }
    }
}