using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Core.JsonConverters
{
    /// <summary>
    /// A JSON converter that (de)serializes an enum to a string using a JsonEnumStringAttribute above each value.
    /// </summary>
    public class JsonEnumStringConverter<T> : JsonConverter where T : struct // TODO - Replace struct with Enum when supported by all IDEs
    {
        private readonly Dictionary<string, List<T>> _readCache = new Dictionary<string, List<T>>();
        private readonly Dictionary<T, string> _writeCache = new Dictionary<T, string>();

        public JsonEnumStringConverter()
        { 
            // Generate cache
            foreach (var field in typeof(T).GetFields())
            {
                var attribute = field.GetCustomAttribute<JsonEnumStringAttribute>();
                if (attribute != null)
                {
                    // Get the string and enum values
                    var strVal = attribute.JsonString;
                    var enumVal = (T) field.GetValue(null);

                    // Store them in the read cache
                    if (!_readCache.ContainsKey(strVal))
                    {
                        _readCache[strVal] = new List<T>();
                    }
                    _readCache[strVal].Add(enumVal);

                    // Store them in the write cache
                    _writeCache[enumVal] = strVal;
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Validation
            if (!CanConvert(value.GetType()))
            {
                throw new JsonException("value must be an Enum of type T");
            }

            var enumVal = (T) value;
            var strVal = _writeCache[enumVal];
            writer.WriteValue(strVal);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var strVal = (string) reader.Value;

            return _readCache[strVal].FirstOrDefault();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}
