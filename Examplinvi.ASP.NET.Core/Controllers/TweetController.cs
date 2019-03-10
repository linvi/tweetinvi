using System;
using System.IO;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Examplinvi.ASP.NET.Core.Controllers
{
    public class TweetController : Controller
    {
        private readonly ITwitterCredentials _credentials;

        public TweetController()
        {
            _credentials = MyCredentials.GenerateCredentials();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string tweet, IFormFile file)
        {
            var fileBytes = GetByteArrayFromFile(file);

            var publishedTweet = Auth.ExecuteOperationWithCredentials(_credentials, () =>
            {
                var publishOptions = new PublishTweetParameters(tweet);
                if (fileBytes != null)
                {
                    publishOptions.MediaBinaries.Add(fileBytes);
                }

                return Tweet.PublishTweet(publishOptions);
            });

            var routeValueParameters = new Dictionary<string, object>();
            routeValueParameters.Add("id", publishedTweet == null ? (Nullable<long>)null : publishedTweet.Id);
            routeValueParameters.Add("actionPerformed", "Publish");
            routeValueParameters.Add("success", publishedTweet != null);
            return RedirectToAction("TweetPublished", routeValueParameters);
        }

        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            file.OpenReadStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public ActionResult TweetPublished(Nullable<long> id, string actionPerformed, bool success = true)
        {
            ViewBag.TweetId = id;
            ViewBag.ActionType = actionPerformed;
            ViewBag.Success = success;
            return View();
        }

        public ActionResult DeleteTweet(long id)
        {
            var success = Auth.ExecuteOperationWithCredentials(_credentials, () =>
            {
                var tweet = Tweet.GetTweet(id);
                if (tweet != null)
                {
                    return tweet.Destroy();
                }

                return false;
            });

            var routeValueParameters = new Dictionary<string, object>();
            routeValueParameters.Add("id", id);
            routeValueParameters.Add("actionPerformed", "Delete");
            routeValueParameters.Add("success", success);
            return RedirectToAction("TweetPublished", routeValueParameters);
        }
    }
}
