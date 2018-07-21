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
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is DateTime)
            {
                return reader.Value;
            }

            var datetimeStr = reader.Value as string;
            DateTime datetime;

            // Some endpoints now return a UNIX epoch (in milliseconds) as the timestamp, so check for that
            if (long.TryParse(datetimeStr, out long epoch))
            {
                var dtOffset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
                datetime = dtOffset.LocalDateTime;
            }
            else // Otherwise it's a formatted string
            {
                datetime = DateTime.ParseExact(datetimeStr, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
            }

            return datetime;
        }
    }
}