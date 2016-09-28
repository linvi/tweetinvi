using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.Model
{
    public class OEmbedTweet : IOEmbedTweet
    {
        public IOEmbedTweetDTO OembedTweetDTO { get; set; }

        public OEmbedTweet(IOEmbedTweetDTO oEmbedTweetDTO)
        {
            OembedTweetDTO = oEmbedTweetDTO;
        }

        public string AuthorName
        {
            get { return OembedTweetDTO.AuthorName; }
        }

        public string AuthorURL
        {
            get { return OembedTweetDTO.AuthorURL; }
        }

        public string HTML
        {
            get { return OembedTweetDTO.HTML; }
        }

        public string URL
        {
            get { return OembedTweetDTO.URL; }
        }

        public string ProviderURL
        {
            get { return OembedTweetDTO.ProviderURL; }
        }

        public double Width
        {
            get { return OembedTweetDTO.Width; }
        }

        public double Height
        {
            get { return OembedTweetDTO.Height; }
        }

        public string Version
        {
            get { return OembedTweetDTO.Version; }
        }

        public string Type
        {
            get { return OembedTweetDTO.Type; }
        }

        public string CacheAge
        {
            get { return OembedTweetDTO.CacheAge; }
        }
    }
}