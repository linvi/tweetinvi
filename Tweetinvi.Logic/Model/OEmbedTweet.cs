using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Logic.Model
{
    public class OEmbedTweet : IOEmbedTweet
    {
        private readonly IOEmbedTweetDTO _oembedTweetDTO;

        public OEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO)
        {
            _oembedTweetDTO = oEmbedTweetDTO;
        }

        public string AuthorName
        {
            get { return _oembedTweetDTO.AuthorName; }
        }

        public string AuthorURL
        {
            get { return _oembedTweetDTO.AuthorURL; }
        }

        public string HTML
        {
            get { return _oembedTweetDTO.HTML; }
        }

        public string URL
        {
            get { return _oembedTweetDTO.URL; }
        }

        public string ProviderURL
        {
            get { return _oembedTweetDTO.ProviderURL; }
        }

        public double Width
        {
            get { return _oembedTweetDTO.Width; }
        }

        public double Height
        {
            get { return _oembedTweetDTO.Height; }
        }

        public string Version
        {
            get { return _oembedTweetDTO.Version; }
        }

        public string Type
        {
            get { return _oembedTweetDTO.Type; }
        }

        public string CacheAge
        {
            get { return _oembedTweetDTO.CacheAge; }
        }
    }
}