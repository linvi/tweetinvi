using System;
using System.Collections.Generic;
using Tweetinvi.Models.Entities.ExtendedEntities;

namespace Tweetinvi.Models.Entities
{
    /// <summary>
    /// Media element posted in Twitter
    /// </summary>
    public interface IMediaEntity : IEquatable<IMediaEntity>
    {
        /// <summary>
        /// Media Id
        /// </summary>
        long? Id { get; set; }

        /// <summary>
        /// Media Id as a string
        /// </summary>
        string IdStr { get; set; }

        /// <summary>
        /// Url of the media
        /// </summary>
        string MediaURL { get; set; }

        /// <summary>
        /// Secured Url of the media
        /// </summary>
        string MediaURLHttps { get; set; }

        /// <summary>
        /// URL information related with the entity
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// URL displayed as it could be displayed as short url
        /// </summary>
        string DisplayURL { get; set; }

        /// <summary>
        /// The expanded URL is the entire URL as opposed to shortened url (bitly...)
        /// </summary>
        string ExpandedURL { get; set; }

        /// <summary>
        /// Type of Media
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// Indicated the location of the entity, for example an URL entity can be located at the begining of the tweet
        /// </summary>
        int[] Indices { get; set; }

        /// <summary>
        /// Dimensions related with the different possible views of 
        /// a same Media element
        /// </summary>
        Dictionary<string, IMediaEntitySize> Sizes { get; set; }

        IVideoInformationEntity VideoDetails { get; set; }
    }
}