using System;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ISavedSearch
    {
        ISavedSearchDTO SavedSearchDTO { get; set; }

        long Id { get; }
        string IdStr { get; }
        string Name { get; set; }
        string Query { get; set; }
        DateTime CreatedAt { get; }
    }
}