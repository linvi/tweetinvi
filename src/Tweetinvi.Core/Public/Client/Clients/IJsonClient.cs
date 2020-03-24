namespace Tweetinvi.Client
{
    public interface IJsonClient
    {
        string Serialize<TFrom>(TFrom obj) where TFrom : class;
        string Serialize<TFrom, TTo>(TFrom obj) where TFrom : class where TTo : class;
        TTo Deserialize<TTo>(string json) where TTo : class;
    }
}