using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using TweetinviCore.Interfaces;

namespace Streaminvi.Helpers
{
    /// <summary>
    /// Operate algorithm on specific object
    /// </summary>
    public static class StreamObjectProcessors
    {
        private static readonly JavaScriptSerializer _jsSerializer;

        static StreamObjectProcessors()
        {
            _jsSerializer = new JavaScriptSerializer();
        }

        /// <summary>
        /// Create a simple method to process objects created by the StreamResultGenerator
        /// </summary>
        /// <param name="processTweetDelegate">Delegate to be used for each new Tweet</param>
        public static Func<string, bool> TweetObjectProcessor(Func<ITweet, bool> processTweetDelegate)
        {
            return delegate(string obj)
            {
                var jsonTweet = _jsSerializer.Deserialize<Dictionary<string, object>>(obj);
                
                if (!String.IsNullOrEmpty(obj))
                {
                    if (jsonTweet.Count() == 1 || !jsonTweet.ContainsKey("id"))
                    {
                        return true;
                    }

                    throw new NotImplementedException();

                    //ITweet t = new Tweet(jsonTweet);
                    //return processTweetDelegate(t);
                }

                // The information sent from Twitter was not the expected object
                return true;
            };
        }
    }
}