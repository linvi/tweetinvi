using System;

namespace Tweetinvi.Core.Attributes
{
    /// <summary>
    /// Attribute against an enum value from Twitter exposing it's JSON string equivelant
    /// </summary>
    public class JsonEnumStringAttribute : Attribute
    {
        public readonly string JsonString;

        public JsonEnumStringAttribute(string jsonString)
        {
            JsonString = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
        }
    }
}
