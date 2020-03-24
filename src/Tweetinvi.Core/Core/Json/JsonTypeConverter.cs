using System;

namespace Tweetinvi.Core.Json
{
    public interface IJsonConverter
    {
        object GetObjectToSerialize(object source);
        object Deserialize(string json);
    }

    public class  JsonTypeConverter<T1, T2> : IJsonConverter
        where T1 : class
        where T2 : class
    {
        private readonly Func<T1, T2> _getDto;
        private readonly Func<string, T1> _deserializer;

        public JsonTypeConverter(Func<T1, T2> getDto, Func<string, T1> deserializer)
        {
            _getDto = getDto;
            _deserializer = deserializer;
        }

        public object GetObjectToSerialize(object source)
        {
            return _getDto(source as T1);
        }

        public object Deserialize(string json)
        {
            return _deserializer(json);
        }
    }
}