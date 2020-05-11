using System;

namespace Tweetinvi.Models.Entities
{
    /// <summary>
    /// https://dev.twitter.com/overview/api/entities-in-twitter-objects#symbols
    /// </summary>
    public interface ISymbolEntity : IEquatable<ISymbolEntity>
    {
        /// <summary>
        /// Text containing the symbol
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The symbol text start and end position
        /// </summary>
        int[] Indices { get; set; }
    }
}