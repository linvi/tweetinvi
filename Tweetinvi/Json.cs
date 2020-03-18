using Tweetinvi.Core.Helpers;

namespace Tweetinvi
{
    public static class SuperJson
    {
        public static string SerializeObject<T>(T obj)
        {
            return TweetinviContainer.Resolve<IJsonObjectConverter>().Serialize(obj);
        }

        public static T DeserializeObject<T>(string json)
        {
            return TweetinviContainer.Resolve<IJsonObjectConverter>().Deserialize<T>(json);
        }
    }
}