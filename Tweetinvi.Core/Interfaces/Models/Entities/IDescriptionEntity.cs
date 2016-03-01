using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models.Entities
{
    public interface IDescriptionEntity
    {
        /// <summary>
        /// URLs found in a description.
        /// </summary>
        IEnumerable<IUrlEntity> Urls { get; set; }
    }
}