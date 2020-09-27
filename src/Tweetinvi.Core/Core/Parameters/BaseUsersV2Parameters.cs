using System.Collections.Generic;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Parameters
{
    public interface IBaseUsersV2Parameters : ICustomRequestParameters
    {
        HashSet<string> Expansions { get; set; }
        HashSet<string> TweetFields { get; set; }
        HashSet<string> UserFields { get; set; }
    }

    public class BaseUsersV2Parameters : CustomRequestParameters, IBaseUsersV2Parameters
    {
        public BaseUsersV2Parameters()
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
            TweetFields = new HashSet<string>();
            UserFields = new HashSet<string>();
        }

        public HashSet<string> Expansions { get; set; }
        public HashSet<string> TweetFields { get; set; }
        public HashSet<string> UserFields { get; set; }
    }
}