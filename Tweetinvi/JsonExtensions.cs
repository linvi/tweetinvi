using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi
{
    public interface IJsonSerializer
    {
        object GetSerializableObject(object source);
        object GetDeserializedObject(string json);
    }


    public class JsonSerializer<T1, T2> : IJsonSerializer
        where T1 : class
        where T2 : class
    {
        private readonly Func<T1, T2> _getSerializableObject;
        private readonly Func<string, T1> _deserializer;

        public JsonSerializer(Func<T1, T2> getSerializableObject, Func<string, T1> deserializer)
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
        private static Dictionary<Type, IJsonSerializer> _getSerializableObject;
        private static Dictionary<Type, IJsonSerializer> _getFromDeserializeObject;

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
            var accountSettingsFactory = TweetinviContainer.Resolve<IAccountController>();

            _getSerializableObject = new Dictionary<Type, IJsonSerializer>();

            // ReSharper disable RedundantTypeArgumentsOfMethod
            Map<ITweet, ITweetDTO>(u => u.TweetDTO, tweetFactory.GenerateTweetFromJson);
            Map<IUser, IUserDTO>(u => u.UserDTO, userFactory.GenerateUserFromJson);
            Map<IMessage, IMessageDTO>(m => m.MessageDTO, messageFactory.GenerateMessageFromJson);
            Map<ITwitterList, ITwitterListDTO>(l => l.TwitterListDTO, twitterListFactory.GenerateListFromJson);
            Map<ISavedSearch, ISavedSearchDTO>(s => s.SavedSearchDTO, savedSearchFactory.GenerateSavedSearchFromJson);
            Map<IAccountSettings, IAccountSettingsDTO>(s => s.AccountSettingsDTO, accountSettingsFactory.GenerateAccountSettingsFromJson);
            // ReSharper restore RedundantTypeArgumentsOfMethod
        }

        public static string ToJson<T>(this T obj) where T : class
        {
            var type = typeof(T);
            object toSerialize = obj;

            var serializer = GetSerializerFromNonCollectionType(type);

            if (serializer != null)
            {
                toSerialize = serializer.GetSerializableObject(obj);
            }
            else if (obj is IEnumerable && type.IsGenericType)
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

                serializer = GetSerializerFromNonCollectionType(genericType);

                if (serializer != null)
                {
                    var enumerable = (IEnumerable)obj;
                    var list = new List<object>();

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
                var serializer = GetSerializerFromNonCollectionType(type);
                if (serializer != null)
                {
                    return serializer.GetDeserializedObject(json) as T;
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

                    serializer = GetSerializerFromNonCollectionType(genericType);
                    if (genericType != null && serializer != null)
                    {
                        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericType));

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

        private static IJsonSerializer GetSerializer(Type type)
        {
            var serializer = GetSerializerFromNonCollectionType(type);

            if (serializer != null)
            {
                return serializer;
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

                if (genericType != null)
                {
                    return GetSerializerFromNonCollectionType(genericType);
                }
            }

            return null;
        }

        private static IJsonSerializer GetSerializerFromNonCollectionType(Type type)
        {
            // Test interfaces
            if (_getSerializableObject.ContainsKey(type))
            {
                return _getSerializableObject[type];
            }

            // Test concrete classes from mapped interfaces
            if (_getSerializableObject.Keys.Any(x => x.IsAssignableFrom(type)))
            {
                return _getSerializableObject.FirstOrDefault(x => x.Key.IsAssignableFrom(type)).Value;
            }

            return null;
        }

        private static void Map<T1, T2>(Func<T1, T2> getSerializableObject, Func<string, T1> deserializer) where T1 : class where T2 : class
        {
            _getSerializableObject.Add(typeof(T1), new JsonSerializer<T1, T2>(getSerializableObject, deserializer));
        }
    }
}