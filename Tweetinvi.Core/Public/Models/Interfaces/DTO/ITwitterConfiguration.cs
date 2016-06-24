using System.Collections.Generic;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models.DTO
{
    public interface ITwitterConfiguration
    {
        int CharactersReservedPerMedia { get; }

        int MessageTextCharacterLimit { get; }
        
        int MaxMediaPerUpload { get; }
        
        string[] NonUsernamePaths { get; }
        
        int PhotoSizeLimit { get; }

        Dictionary<string, IMediaEntitySize> PhotoSizes { get; }

        int ShortURLLength { get; }
        
        int ShortURLLengthHttps { get; }
    }
}