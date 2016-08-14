using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi
{
    public interface IJsonMap
    {
        object GetSerializableObject(object source);
        object GetDeserializedObject(string json);
    }


    public class JsonMap<T1, T2> : IJsonMap
        where T1 : class
        where T2 : class
    {
        private readonly Func<T1, T2> _getSerializableObject;
        private readonly Func<string, T1> _deserializer;

        public JsonMap(Func<T1, T2> getSerializableObject, Func<string, T1> deserializer)
        {
            _getSerializableObject = getSerializableObject;
            _deserializer = deserializer;
        }

        public object GetSerializableObject(object source)
        {
            return _getSerializableObject(source as T1);
        }

        public object GetDeserializedObject(string json)
        {
            return _deserializer(json);
        }
    }

    public static class JsonExtensions
    {
        private static MethodInfo _jsonConvert;
        private static Dictionary<Type, IJsonMap> _getSerializableObject;
        private static Dictionary<Type, IJsonMap> _getFromDeserializeObject;

        static JsonExtensions()
        {
            _jsonConvert = typeof(JsonConvert)
                         .GetMethods()
                         .Where(m => m.Name == "DeserializeObject")
                         .Select(m => new
                         {
                             Method = m,
                             Params = m.GetParameters(),
                             Args = m.GetGenericArguments()
                         })
                         .Where(x => x.Args.Length == 1 &&
                                     x.Params.Length == 2)
                         .Select(x => x.Method)
                         .First();

            var tweetFactory = TweetinviContainer.Resolve<ITweetFactory>();
            var userFactory = TweetinviContainer.Resolve<IUserFactory>();
            var messageFactory = TweetinviContainer.Resolve<IMessageFactory>();
            var twitterListFactory = TweetinviContainer.Resolve<ITwitterListFactory>();
            var savedSearchFactory = TweetinviContainer.Resolve<ISavedSearchFactory>();

            _getSerializableObject = new Dictionary<Type, IJsonMap>();

            // ReSharper disable RedundantTypeArgumentsOfMethod
            Map<ITweet, ITweetDTO>(u => u.TweetDTO, tweetFactory.GenerateTweetFromJson);
            Map<IUser, IUserDTO>(u => u.UserDTO, userFactory.GenerateUserFromJson);
            Map<IMessage, IMessageDTO>(m => m.MessageDTO, messageFactory.GenerateMessageFromJson);
            Map<ITwitterList, ITwitterListDTO>(l => l.TwitterListDTO, twitterListFactory.GenerateListFromJson);
            Map<ISavedSearch, ISavedSearchDTO>(s => s.SavedSearchDTO, savedSearchFactory.GenerateSavedSearchFromJson);
            // ReSharper restore RedundantTypeArgumentsOfMethod
        }

        public static string ToJson<T>(this T obj) where T : class
        {
            var type = typeof(T);
            object toSerialize = obj;

            if (_getSerializableObject.ContainsKey(type))
            {
                toSerialize = _getSerializableObject[type].GetSerializableObject(obj);
            }
            else if (obj is IEnumerable && type.IsGenericType)
            {
                var enumerable = (IEnumerable)obj;
                var genericType = type.GetGenericArguments()[0];
                if (_getSerializableObject.ContainsKey(genericType))
                {
                    var list = new List<object>();
                    var serializer = _getSerializableObject[genericType];

                    foreach (var o in enumerable)
                    {
                        list.Add(serializer.GetSerializableObject(o));
                    }

                    toSerialize = list;
                }
            }

            try
            {
                return JsonConvert.SerializeObject(toSerialize);
            }
            catch (Exception ex)
            {
                throw new Exception("The type provided is probably not compatible with Tweetinvi Json serializer.", ex);
            }
        }

        public static T ConvertJsonTo<T>(this string json) where T : class
        {
            var type = typeof(T);

            try
            {
                if (_getSerializableObject.ContainsKey(type))
                {
                    return _getSerializableObject[type].GetDeserializedObject(json) as T;
                }

                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    Type genericType = null;

                    if (type.IsGenericType)
                    {
                        genericType = type.GetGenericArguments()[0];
                    }
                    else if (typeof(Array).IsAssignableFrom(type))
                    {
                        genericType = type.GetElementType();
                    }

                    if (_getSerializableObject.ContainsKey(genericType))
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));
                        var serializer = _getSerializableObject[genericType];

                        JArray jsonArray = JArray.Parse(json);

                        foreach (var elt in jsonArray)
                        {
                            var eltJson = elt.ToJson();
                            list.Add(serializer.GetDeserializedObject(eltJson));
                        }

                        if (typeof(Array).IsAssignableFrom(type))
                        {
                            var array = Array.CreateInstance(genericType, list.Count);
                            list.CopyTo(array, 0);
                            return array as T;
                        }

                        return list as T;
                    }
                }


                return JsonConvert.DeserializeObject<T>(json, JsonPropertiesConverterRepository.Converters);
            }
            catch (Exception ex)
            {
                throw new Exception("The type provided is probably not compatible with Tweetinvi Json serializer.", ex);
            }
        }

        private static void Map<T1, T2>(Func<T1, T2> getSerializableObject, Func<string, T1> deserializer) where T1 : class where T2 : class
        {
            _getSerializableObject.Add(typeof(T1), new JsonMap<T1, T2>(getSerializableObject, deserializer));
        }
    }
}