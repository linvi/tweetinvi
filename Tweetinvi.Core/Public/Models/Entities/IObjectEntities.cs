using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tweetinvi.Models.Entities
{
    public interface IObjectEntities
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

        /// <summary>
        /// Collection of medias associated with a Tweet
        /// </summary>
        List<IMediaEntity> Medias { get; }
    }
}