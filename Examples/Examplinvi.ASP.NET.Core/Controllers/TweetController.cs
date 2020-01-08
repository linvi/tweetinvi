using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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
        public async Task<ActionResult> Index(string tweet, IFormFile file)
        {
            var fileBytes = GetByteArrayFromFile(file);
            var client = new TwitterClient(_credentials);

            try
            {
                var publishTweetParameters = new PublishTweetParameters(tweet);

                if (fileBytes != null)
                {
                    var media = await client.Upload.UploadBinary(fileBytes);
                    publishTweetParameters.Medias.Add(media);
                }

                var publishedTweet = await client.Tweets.PublishTweet(publishTweetParameters);
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", publishedTweet?.Id },
                    { "author", publishedTweet?.CreatedBy.ScreenName },
                    { "actionPerformed", "Publish" },
                    { "success", publishedTweet != null }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }
            catch (Exception)
            {
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", null },
                    { "author", null },
                    { "actionPerformed", "Publish" },
                    { "success", false }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }
        }

        private static byte[] GetByteArrayFromFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            file.OpenReadStream().CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public ActionResult TweetPublished(long? id, string author, string actionPerformed, bool success = true)
        {
            ViewBag.TweetId = id;
            ViewBag.Author = author;
            ViewBag.ActionType = actionPerformed;
            ViewBag.Success = success;
            return View();
        }

        public async Task<ActionResult> DeleteTweet(long id)
        {
            var client = new TwitterClient(_credentials);

            try
            {
                var tweet = await client.Tweets.GetTweet(id);
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", id },
                    { "author", tweet.CreatedBy.ScreenName },
                    { "actionPerformed", "Delete" },
                    { "success", "true" }
                };

                try
                {
                    await tweet.Destroy();
                }
                catch (Exception e)
                {
                    routeValueParameters["success"] = "false";
                }

                return RedirectToAction("TweetPublished", routeValueParameters);
            }
            catch (Exception)
            {
                var routeValueParameters = new Dictionary<string, object>
                {
                    { "id", id },
                    { "actionPerformed", "Delete" },
                    { "success", false }
                };

                return RedirectToAction("TweetPublished", routeValueParameters);
            }
        }
    }
}