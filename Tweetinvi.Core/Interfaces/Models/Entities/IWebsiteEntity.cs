using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IWebsiteEntity
    {
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}