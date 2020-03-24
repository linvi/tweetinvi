using System;

namespace Tweetinvi.Models.DTO
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