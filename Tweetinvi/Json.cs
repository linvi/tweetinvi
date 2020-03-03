using Tweetinvi.Core.Helpers;

namespace Tweetinvi
{
    public static class SuperJson
    {
        public static string SerializeObject<T>(T obj)
        {
            return TweetinviContainer.Resolve<IJsonObjectConverter>().SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            return TweetinviContainer.Resolve<IJsonObjectConverter>().DeserializeObject<T>(json);
        }
    }
}