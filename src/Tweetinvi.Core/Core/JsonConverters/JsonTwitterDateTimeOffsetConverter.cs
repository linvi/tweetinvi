using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tweetinvi.Core.JsonConverters
{
    public class JsonTwitterDateTimeOffsetConverter : DateTimeConverterBase
    {
        private readonly string _format;

        public JsonTwitterDateTimeOffsetConverter() : this("ddd MMM dd HH:mm:ss zzzz yyyy")
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global -- This is used by JsonConverters
        public JsonTwitterDateTimeOffsetConverter(string format)
        {
            _format = format;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is DateTimeOffset)
            {
                return reader.Value;
            }

            if (reader.Value is DateTime)
            {
                return new DateTimeOffset((DateTime)reader.Value);
            }

            var datetimeStr = reader.Value as string;
            DateTimeOffset datetime;

            // Some endpoints now return a UNIX epoch (in milliseconds) as the timestamp, so check for that
            if (long.TryParse(datetimeStr, out long epoch))
            {
                var dtOffset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
                datetime = dtOffset.LocalDateTime;
            }
            else // Otherwise it's a formatted string
            {
                datetime = DateTimeOffset.ParseExact(datetimeStr, _format, CultureInfo.InvariantCulture);
            }

            return datetime;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?);
        }
    }
}