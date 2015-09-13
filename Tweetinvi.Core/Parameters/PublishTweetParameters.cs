using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Parameters.QueryParameters;

namespace Tweetinvi.Core.Parameters
{
    public class PublishTweetParameters : IPublishTweetParameters
    {
        public PublishTweetParameters(string text, IPublishTweetOptionalParameters optionalParameters = null)
        {
            Text = text;

            if (optionalParameters == null)
            {
                Parameters = new PublishTweetOptionalParameters();
            }
            else
            {
                Parameters = optionalParameters;
            }
        }

        public string Text { get; private set; }
        public IPublishTweetOptionalParameters Parameters { get; private set; }

        public long? InReplyToTweetId
        {
            get { return Parameters.InReplyToTweetId; }
            set { Parameters.InReplyToTweetId = value; }
        }

        public ITweetIdentifier InReplyToTweet
        {
            get { return Parameters.InReplyToTweet; }
            set { Parameters.InReplyToTweet = value; }
        }

        public string PlaceId
        {
            get { return Parameters.PlaceId; }
            set { Parameters.PlaceId = value; }
        }

        public ICoordinates Coordinates
        {
            get { return Parameters.Coordinates; }
            set { Parameters.Coordinates = value; }
        }

        public ITweet QuotedTweet
        {
            get { return Parameters.QuotedTweet; }
            set { Parameters.QuotedTweet = value; }
        }

        public bool? DisplayExactCoordinates
        {
            get { return Parameters.DisplayExactCoordinates; }
            set { Parameters.DisplayExactCoordinates = value; }
        }

        public List<long> MediaIds
        {
            get { return Parameters.MediaIds; }
        }

        public List<IMedia> Medias
        {
            get { return Parameters.Medias; }
        }

        public List<byte[]> MediaBinaries
        {
            get { return Parameters.MediaBinaries; }
        }

        public bool? PossiblySensitive
        {
            get { return Parameters.PossiblySensitive; }
            set { Parameters.PossiblySensitive = value; }
        }

        public bool? TrimUser
        {
            get { return Parameters.TrimUser; }
            set { Parameters.TrimUser = value; }
        }

        public List<Tuple<string, string>> CustomQueryParameters
        {
            get { return Parameters.CustomQueryParameters; }
        }

        public string FormattedCustomQueryParameters
        {
            get { return Parameters.FormattedCustomQueryParameters; }
        }

        public void AddCustomQueryParameter(string name, string value)
        {
            Parameters.AddCustomQueryParameter(name, value);
        }

        public void ClearCustomQueryParameters()
        {
            Parameters.ClearCustomQueryParameters();
        }
    }
}