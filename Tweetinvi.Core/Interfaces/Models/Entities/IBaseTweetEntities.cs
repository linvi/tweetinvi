using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IBaseTweetEntities
    {
        /// <summary>
        /// Collection of urls associated with a Tweet
        /// </summary>
        List<IUrlEntity> Urls { get; }

        /// <summary>
        /// Collection of tweets mentioning this Tweet
        /// </summary>
        List<IUserMentionEntity> UserMentions { get; }

        /// <summary>
        /// Collection of hashtags associated with a Tweet
        /// </summary>
        List<IHashtagEntity> Hashtags { get; }

        /// <summary>
        /// Collection of symbols associated with a Tweet
        /// </summary>
        List<ISymbolEntity> Symbols { get; }
    }
}