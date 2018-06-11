using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Core.Helpers;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Logic.JsonConverters
{
    /// <summary>
    /// A JSON converter that (de)serializes an enum to a string using a JsonEnumStringAttribute above each value.
    /// </summary>
    public class JsonEnumStringConverter<T> : JsonConverter where T : struct // TODO - Replace struct with Enum when supported by all IDEs
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Validation
            if (!CanConvert(value.GetType()))
            {
                throw new JsonException("value must be an Enum of type T");
            }

            var enumVal = (T)value;
            var strVal = enumVal.GetAttributeOfType<JsonEnumStringAttribute>().JsonString;
            writer.WriteValue(strVal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var strVal = (string)reader.Value;

            // Try and get the corresponding enum value, will return default(T) if none found
            EnumHelpers.TryGetValueWhereAttribute<T, JsonEnumStringAttribute>(a => a.JsonString == strVal, out T val);
            return val;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
