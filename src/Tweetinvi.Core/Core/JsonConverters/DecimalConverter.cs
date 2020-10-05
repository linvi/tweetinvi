using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tweetinvi.Core.JsonConverters
{
    public class DecimalConverter : JsonConverter
    {
        public static readonly DecimalConverter Instance = new DecimalConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                if (objectType == typeof(decimal?))
                {
                    return null;
                }

                return default(decimal);
            }

            // ReSharper disable once StyleCop.SA1126
            if (decimal.TryParse(value, out var result))
            {
                // ReSharper disable once StyleCop.SA1126
                return result;
            }

            if (objectType == typeof(decimal?))
            {
                return null;
            }

            return default(decimal);
        }

        /// <inheritdoc cref="JsonConverter"/>
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <seealso cref="JsonConverter"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var d = default(decimal?);

            if (value != null)
            {
                d = value as decimal?;
                if (d.HasValue)
                {
                    d = new decimal(decimal.ToDouble(d.Value));
                }
            }

            JToken.FromObject(d ?? 0).WriteTo(writer);
        }
    }
}