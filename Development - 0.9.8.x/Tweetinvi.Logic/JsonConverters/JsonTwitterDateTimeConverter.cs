using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonTwitterDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is DateTime)
            {
                return reader.Value;
            }

            var datetimeStr = reader.Value as string;
            var datetime = DateTime.ParseExact(datetimeStr, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);

            return datetime;
        }
    }
}