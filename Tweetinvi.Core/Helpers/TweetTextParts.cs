using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Helpers
{
    public class TweetTextParts : ITweetTextParts
    {
        public TweetTextParts(string text)
        {
            var stringMatches = Regex.Match(text, @"^(?<prefix>(?:(?<mention>@[a-zA-Z0-9]+)\s){0,50})?(?<content>.+)");

            var prefix = stringMatches.Groups["prefix"];
            var content = stringMatches.Groups["content"];

            Prefix = prefix.Value;
            Content = content.Value;

            var mentionCaptures = stringMatches.Groups["mention"].Captures;

            var mentions = new List<string>();
            foreach (var mention in mentionCaptures)
            {
                mentions.Add(mention.ToString());
            }

            Mentions = mentions.ToArray();
        }

        public string Content { get; private set; }
        public string Prefix { get; private set; }
        public string[] Mentions { get; private set; }
    }
}