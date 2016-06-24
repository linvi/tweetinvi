using System;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.Model
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
            get { return _savedSearchDTO; }
            set { _savedSearchDTO = value; }
        }

        public long Id
        {
            get { return _savedSearchDTO.Id; }
        }

        public string IdStr
        {
            get { return _savedSearchDTO.IdStr; }
        }

        public string Name
        {
            get { return _savedSearchDTO.Name; }
            set { _savedSearchDTO.Name = value; }
        }

        public string Query
        {
            get { return _savedSearchDTO.Query; }
            set { _savedSearchDTO.Query = value; }
        }

        public DateTime CreatedAt
        {
            get { return _savedSearchDTO.CreatedAt; }
        }
    }
}