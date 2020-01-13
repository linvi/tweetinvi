namespace Tweetinvi.Client.Tools
{
    public interface ITwitterClientJson
    {
        T DeserializeObject<T>(string json);
    }
}