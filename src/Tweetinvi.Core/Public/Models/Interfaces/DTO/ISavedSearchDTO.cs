using System;

namespace Tweetinvi.Models.DTO
{
    public interface ISavedSearchDTO
    {
        long Id { get; set; }
        string IdStr { get; set; }
        string Name { get; set; }
        string Query { get; set; }
        DateTimeOffset CreatedAt { get; set; }
    }
}