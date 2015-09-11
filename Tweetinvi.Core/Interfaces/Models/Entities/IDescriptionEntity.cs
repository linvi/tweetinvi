using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IDescriptionEntity
    {
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}