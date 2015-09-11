using System;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface ISavedSearchDTO
    {
        long Id { get; set; }
        string IdStr { get; set; }
        string Name { get; set; }
        string Query { get; set; }
        DateTime CreatedAt { get; set; }
    }
}