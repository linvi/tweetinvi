using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Core.Interfaces.DTO
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