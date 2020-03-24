using System.Collections.Generic;

namespace Tweetinvi.Models.Entities
{
    public interface IWebsiteEntity
    {
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}