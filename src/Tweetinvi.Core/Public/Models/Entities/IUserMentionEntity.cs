using System;
using System.Collections.Generic;

namespace Tweetinvi.Models.Entities
{
    /// <summary>
    /// Basic information related with a User and provided
    /// in twitter objects like Tweets
    /// </summary>
    public interface IUserMentionEntity : IEquatable<IUserMentionEntity>
    {
        /// <summary>
        /// User Id
        /// </summary>
        long? Id { get; set; }

        /// <summary>
        /// User Id as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// User ScreenName
        /// </summary>
        string ScreenName { get; set; }

        /// <summary>
        /// User displayed name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// An array of integers indicating the offsets within 
        /// the TwitterObject where the hashtag begins and ends.
        /// </summary>
        IList<int> Indices { get; set; }
    }
}