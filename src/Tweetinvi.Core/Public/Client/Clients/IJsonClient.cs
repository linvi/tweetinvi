namespace Tweetinvi.Client
{
    public interface IJsonClient
    {
        /// <summary>
        /// Serializes a Twitter object in such a way that it can be deserialized by Tweetinvi
        /// </summary>
        /// <typeparam name="TFrom">Type of the object to serialize</typeparam>
        /// <returns>Json serialized object</returns>
        string Serialize<TFrom>(TFrom obj) where TFrom : class;

        /// <summary>
        /// Serializes a Twitter object in such a way that it can be deserialized by Tweetinvi
        /// </summary>
        /// <typeparam name="TFrom">Type of the object to serialize</typeparam>
        /// <typeparam name="TTo">Type that the object will be serialized</typeparam>
        /// <returns>Json serialized object</returns>
        string Serialize<TFrom, TTo>(TFrom obj) where TFrom : class where TTo : class;

        /// <summary>
        /// Deserializes json into a dto object
        /// </summary>
        /// <returns>DTO</returns>
        TTo Deserialize<TTo>(string json) where TTo : class;
    }
}