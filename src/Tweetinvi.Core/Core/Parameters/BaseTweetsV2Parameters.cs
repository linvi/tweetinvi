using System.Collections.Generic;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Parameters
{
    public interface IBaseTweetsV2Parameters : ICustomRequestParameters
    {
        HashSet<string> Expansions { get; set; }
        HashSet<string> MediaFields { get; set; }
        HashSet<string> PlaceFields { get; set; }
        HashSet<string> PollFields { get; set; }
        HashSet<string> TweetFields { get; set; }
        HashSet<string> UserFields { get; set; }
    }

    public class BaseTweetsV2Parameters : CustomRequestParameters, IBaseTweetsV2Parameters
    {
        public BaseTweetsV2Parameters()
        {
            InitializeAllFields();
            this.WithAllFields();
        }

        public virtual void ClearAllFields()
        {
            InitializeAllFields();
        }

        private void InitializeAllFields()
        {
            Expansions = new HashSet<string>();
            MediaFields = new HashSet<string>();
            PlaceFields = new HashSet<string>();
            PollFields = new HashSet<string>();
            TweetFields = new HashSet<string>();
            UserFields = new HashSet<string>();
        }

        public HashSet<string> Expansions { get; set; }
        public HashSet<string> MediaFields { get; set; }
        public HashSet<string> PlaceFields { get; set; }
        public HashSet<string> PollFields { get; set; }
        public HashSet<string> TweetFields { get; set; }
        public HashSet<string> UserFields { get; set; }
    }
}