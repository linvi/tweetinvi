using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class PublishTweetOptionalParameters : CustomRequestParameters, IPublishTweetOptionalParameters
    {
        private ITweetIdentifier _tweetIdentifier;

        public PublishTweetOptionalParameters()
        {
            MediaIds = new List<long>();
            Medias = new List<IMedia>();
            MediaBinaries = new List<byte[]>();
        }

        public ITweetIdentifier InReplyToTweet
        {
            get { return _tweetIdentifier; }
            set
            {
                if (value != null && value.Id == -1)
                {
                    throw new InvalidOperationException("You cannot reply to a tweet that has not yet been published!");
                }

                _tweetIdentifier = value;
            }
        }

        public ITweet QuotedTweet { get; set; }

        public long? InReplyToTweetId
        {
            get { return InReplyToTweet != null ? (long?)InReplyToTweet.Id : null; }
            set
            {
                if (value != null)
                {
                    InReplyToTweet = new TweetIdentifier((long)value);
                }
                else
                {
                    InReplyToTweet = null;
                }
            }
        }

        public List<long> MediaIds { get; set; }
        public List<IMedia> Medias { get; set; }
        public List<byte[]> MediaBinaries { get; set; }

        public string PlaceId { get; set; }
        public ICoordinates Coordinates { get; set; }
        public bool? DisplayExactCoordinates { get; set; }

        public bool? PossiblySensitive { get; set; }
        public bool? TrimUser { get; set; }
    }
}