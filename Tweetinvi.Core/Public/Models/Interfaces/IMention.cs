namespace Tweetinvi.Models
{
    /// <summary>
    /// Twitter mention
    /// </summary>
    public interface IMention : ITweet
    {
        // Notice that IMention inherits froms ITweet
        #region IMention Properties
        
        /// <summary>
        /// Mention annotation
        /// </summary>
        string Annotations { get; set; } 

        #endregion
    }
}