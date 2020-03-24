using System.Collections.Generic;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Set of methods to retrieve suggested users
    /// </summary>
    public interface ISuggestedUserList
    {
        /// <summary>
        /// List name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name of the list or category
        /// </summary>
        string Slug { get; set; }
        
        /// <summary>
        /// Size of the list
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// Suggested users
        /// </summary>
        List<IUser> Members { get; set; }
    }
}