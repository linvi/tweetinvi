using System.Collections.Generic;

namespace Tweetinvi.Models.Entities
{
    public interface IWebsiteEntity
    {
        /// <summary>
        /// Website urls
        /// </summary>
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}