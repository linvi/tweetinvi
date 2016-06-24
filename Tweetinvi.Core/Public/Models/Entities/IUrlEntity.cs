using System;

namespace Tweetinvi.Models.Entities
{
    /// <summary>
    /// Information related with an URL in twitter
    /// </summary>
    public interface IUrlEntity : IEquatable<IUrlEntity>
    {
        /// <summary>
        /// Real url
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Message displayed instead of the url
        /// </summary>
        string DisplayedURL { get; set; }

        /// <summary>
        /// The fully resolved URL
        /// </summary>
        string ExpandedURL { get; set; }

        /// <summary>
        /// The character positions the url was extracted from
        /// </summary>
        int[] Indices { get; set; }
    }
}