using System;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Models.Properties
{
    public class SavedSearch : ISavedSearch
    {
        private ISavedSearchDTO _savedSearchDTO;

        public SavedSearch(ISavedSearchDTO savedSearchDTO)
        {
            _savedSearchDTO = savedSearchDTO;
        }

        public ISavedSearchDTO SavedSearchDTO
        {
            get => _savedSearchDTO;
            set => _savedSearchDTO = value;
        }

        public long Id => _savedSearchDTO.Id;

        public string IdStr => _savedSearchDTO.IdStr;

        public string Name
        {
            get => _savedSearchDTO.Name;
            set => _savedSearchDTO.Name = value;
        }

        public string Query
        {
            get => _savedSearchDTO.Query;
            set => _savedSearchDTO.Query = value;
        }

        public DateTime CreatedAt => _savedSearchDTO.CreatedAt;
    }
}