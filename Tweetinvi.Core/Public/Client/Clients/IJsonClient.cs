namespace Tweetinvi.Client.Tools
{
    public interface IJsonClient
    {
        string Serialize<T>(T obj) where T : class;
        T Deserialize<T>(string json) where T : class;
    }
}