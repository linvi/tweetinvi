using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Json;
using SavedSearch = Tweetinvi.SavedSearch;
using Stream = Tweetinvi.Stream;

namespace Examplinvi
{
    // IMPORTANT 
    // This cheat sheet provide examples for all the features provided by Tweetinvi.

    // WINDOWS PHONE 8 developers
    // If you are a windows phone developer, please use the Async classes
    // User.GetAuthenticatedUser(); -> await UserAsync.GetAuthenticatedUser();

    class Program
    {
        static void Main()
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");

            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine(args.QueryURL);
            };

            var authenticatedUser = User.GetAuthenticatedUser();

            GenerateCredentialExamples();
            UserLiveFeedExamples();
            TweetExamples();
            UserExamples();
            AuthenticatedUserExamples();
            TimelineExamples();
            MessageExamples();
            TwitterListExamples();
            GeoExamples();
            SearchExamples();
            SavedSearchesExamples();
            RateLimitExamples();
            HelpExamples();
            JsonExamples();
            StreamExamples();
            AdditionalFeaturesExamples();
            Examples.ConfigureTweetinvi();
            Examples.GlobalEvents();
            UploadExamples();

            Console.WriteLine(@"END");
            Console.ReadLine();
        }

        #region Examples Store

        // ReSharper disable LocalizableElement
        // ReSharper disable UnusedMember.Local

        private static void GenerateCredentialExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            // With captcha
            Examples.AuthFlow_WithCaptcha_StepByStep("consumer_key", "consumer_secret");

            // With callback URL
            Examples.AuthFlow_CreateFromRedirectedCallbackURL_StepByStep("consumer_key", "consumer_secret");

            Examples.AuthFlow_CreateFromRedirectedVerifierCode_StepByStep("consumer_key", "consumer_secret");
        }

        private static void UserLiveFeedExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Stream_UserStreamExample();
        }

        private static void TweetExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Tweet_GetExistingTweet(210462857140252672);

            Examples.Tweet_PublishTweet(string.Format("I love tweetinvi! ({0})", Guid.NewGuid()));
            Examples.Tweet_PublishTweetWithImage("Uploadinvi?", "YOUR_FILE_PATH.png");

            Examples.Tweet_PublishTweetInReplyToAnotherTweet(string.Format("I love tweetinvi! ({0})", Guid.NewGuid()), 392711547081854976);
            Examples.Tweet_PublishTweetWithGeo(string.Format("I love tweetinvi! ({0})", Guid.NewGuid()));

            Examples.Tweet_Destroy();

            Examples.Tweet_GetRetweets(210462857140252672);
            Examples.Tweet_PublishRetweet(210462857140252672);
            Examples.Tweet_DestroyRetweet(210462857140252672);

            Examples.Tweet_GenerateOEmbedTweet();
            Examples.Tweet_SetTweetAsFavorite(392711547081854976);
        }

        private static void UserExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.User_GetCurrentUser();

            Examples.User_GetUserFromId(1478171);
            Examples.User_GetUserFromName(Examples.USER_SCREEN_NAME_TO_TEST);

            Examples.User_GetFavorites(Examples.USER_SCREEN_NAME_TO_TEST);

            Examples.User_GetFriends(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_GetFriendIds(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_GetFriendIdsUpTo(Examples.USER_SCREEN_NAME_TO_TEST, 10000);

            Examples.User_GetFollowers(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_GetFollowerIds(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_GetFollowerIdsUpTo(Examples.USER_SCREEN_NAME_TO_TEST, 10000);

            Examples.User_GetRelationshipBetween("tweetinvitest", Examples.USER_SCREEN_NAME_TO_TEST);

            Examples.User_BlockUser(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_UnBlockUser(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.User_GetBlockedUsers();

            Examples.User_DownloadProfileImage(Examples.USER_SCREEN_NAME_TO_TEST);
        }

        private static void AuthenticatedUserExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Friendship_GetMultipleRelationships();
            Examples.AuthenticatedUser_GetIncomingRequests();
            Examples.AuthenticatedUser_GetOutgoingRequests();
            Examples.AuthenticatedUser_FollowUser(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.AuthenticatedUser_UnFollowUser(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.AuthenticatedUser_UpdateFollowAuthorizationsForUser(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.AuthenticatedUser_GetLatestReceivedMessages();
            Examples.AuthenticatedUser_GetLatestSentMessages();
            Examples.AuthenticatedUser_GetAccountSettings();
        }

        private static void TimelineExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Timeline_GetHomeTimeline();
            Examples.Timeline_GetUserTimeline(Examples.USER_SCREEN_NAME_TO_TEST);
            Examples.Timeline_GetMentionsTimeline();
        }

        private static void MessageExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.AuthenticatedUser_GetLatestReceivedMessages();
            Examples.AuthenticatedUser_GetLatestSentMessages();

            Examples.Message_GetLatests();
            Examples.Message_GetMessageFromId(381069551028293633);
            Examples.Message_PublishMessage("I love tweetinvi", Examples.USER_SCREEN_NAME_TO_TEST);
        }

        private static void StreamExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Stream_SampleStreamExample();
            Examples.Stream_FilteredStreamExample();
            Examples.Stream_UserStreamExample();
            Examples.SimpleStream_Events();
        }

        private static void TwitterListExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.TwitterList_GetUserOwnedLists();
            Examples.TwitterList_GetUserSubscribedLists();

            Examples.TwitterList_CreateList();
            Examples.TwitterList_GetExistingListById(105601767);
            Examples.TwitterList_UpdateList(105601767);
            Examples.TwitterList_DestroyList(105601767);
            Examples.TwitterList_GetTweetsFromList(105601767);
            Examples.TwitterList_GetMembersOfList(105601767);
        }

        private static void GeoExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Geo_GetPlaceFromId("df51dec6f4ee2b2c");
            Examples.Trends_GetTrendsFromWoeId(1);
        }

        private static void SearchExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Search_SimpleTweetSearch();
            Examples.Search_SearchTweet();
            Examples.Search_SearchWithMetadata();
            Examples.Search_FilteredSearch();
            Examples.Search_SearchUsers();
        }

        private static void SavedSearchesExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.SavedSearch_CreateSavedSearch("tweetinvi");
            Examples.SavedSearch_GetSavedSearches();
            Examples.SavedSearch_GetSavedSearch(307102135);
            Examples.SavedSearch_DestroySavedSearch(307102135);
        }

        private static void RateLimitExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.RateLimits_Track_Examples();
            Examples.RateLimits_ManualAwait();

            Examples.GetCredentialsRateLimits();
            Examples.GetCurrentCredentialsRateLimits();
        }

        private static void HelpExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.GetTwitterPrivacyPolicy();
        }

        private static void JsonExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Json_GetJsonForAccountRequestExample();
            Examples.Json_GetJsonForMessageRequestExample();
            Examples.Json_GetJsonCursorRequestExample();
            Examples.Json_GetJsonForGeoRequestExample();
            Examples.Json_GetJsonForHelpRequestExample();
            Examples.Json_GetJsonForSavedSearchRequestExample();
            Examples.Json_GetJsonForTimelineRequestExample();
            Examples.Json_GetJsonForTrendsRequestExample();
            Examples.Json_GetJsonForTweetRequestExample();
            Examples.Json_GetJsonForUserRequestExample();
        }

        private static void AdditionalFeaturesExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.Exceptions_GetExceptionsInfo();
            Examples.ManualQuery_Example();
        }

        private static void UploadExamples()
        {
            if (!Examples.ExecuteExamples)
            {
                return;
            }

            Examples.ChunkedUpload(new byte[10], "video/mp4");
            Examples.Tweet_PublishTweetWithImage("publish with img", "filePath");
        }

        #endregion
    }

    static class Examples
    {
        public static bool ExecuteExamples { get; set; }
        public const string USER_SCREEN_NAME_TO_TEST = "ladygaga";

        #region Credentials and Login

        // Get credentials with captcha system
        // ReSharper disable UnusedMethodReturnValue.Local
        public static ITwitterCredentials AuthFlow_WithCaptcha_StepByStep(string consumerKey, string consumerSecret)
        {
var applicationCredentials = new ConsumerCredentials(consumerKey, consumerSecret);
var authenticationContext = AuthFlow.InitAuthentication(applicationCredentials);
Console.WriteLine("Go on : {0}", authenticationContext.AuthorizationURL);
Console.WriteLine("Enter the captch : ");
var captcha = Console.ReadLine();

try
{
    var newCredentials = AuthFlow.CreateCredentialsFromVerifierCode(captcha, authenticationContext);
    Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
    Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

    return newCredentials;
}
catch (Exception)
{
    return null;
}
        }

        // Get credentials with callbackURL system
        public static ITwitterCredentials AuthFlow_CreateFromRedirectedCallbackURL_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = new ConsumerCredentials(consumerKey, consumerSecret);
            var authenticationContext = AuthFlow.InitAuthentication(applicationCredentials, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Go on : {0}", authenticationContext);
            Console.WriteLine("When redirected to your website copy and paste the URL: ");

            // Enter a value like: https://tweeetinvi.codeplex.com?oauth_token={tokenValue}&oauth_verifier={verifierValue}

            var callbackURL = Console.ReadLine();

            // Here we provide the entire URL where the user has been redirected
            var newCredentials = AuthFlow.CreateCredentialsFromCallbackURL(callbackURL, authenticationContext);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        public static ITwitterCredentials AuthFlow_CreateFromRedirectedVerifierCode_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = new ConsumerCredentials(consumerKey, consumerSecret);
            var authenticationContext = AuthFlow.InitAuthentication(applicationCredentials, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Go on : {0}", authenticationContext);
            Console.WriteLine("When redirected to your website copy and paste the value of the oauth_verifier : ");

            // For the following redirection https://tweetinvi.codeplex.com?oauth_token=UR3eTEwDXFNhkMnjqz0oFbRauvAm4YhnF67KE6hO8Q&oauth_verifier=woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A
            // Enter the value : woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A

            var verifierCode = Console.ReadLine();

            // Here we only provide the verifier code
            var newCredentials = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authenticationContext);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        // ReSharper restore UnusedMethodReturnValue.Local


        #endregion

        #region Tweet

        public static void Tweet_GetExistingTweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            Console.WriteLine(tweet.Text);
        }

        public static void Tweet_PublishTweet(string text)
        {
            var tweet = Tweet.PublishTweet(text);
            Console.WriteLine(tweet.IsTweetPublished);
        }

        public static ITweet Tweet_PublishTweetWithImage(string text, string filePath, string filepath2 = null)
        {
            byte[] file1 = File.ReadAllBytes(filePath);

            var publishMultipleImages = filepath2 != null;

            // Create a tweet with a single image
            if (publishMultipleImages)
            {
                var publishParameters = new PublishTweetOptionalParameters
                {
                    MediaBinaries = new[]
                    {
                        file1,
                        File.ReadAllBytes(filepath2)
                    }.ToList()
                };

                return Tweet.PublishTweet(text, publishParameters);
            }
            else
            {
                return Tweet.PublishTweetWithImage(text, file1);
            }
        }

        public static void Tweet_PublishTweetInReplyToAnotherTweet(string text, long tweetIdtoReplyTo)
        {
            var tweetToReplyTo = Tweet.GetTweet(tweetIdtoReplyTo);

            // We must add @screenName of the author of the tweet we want to reply to
            var textToPublish = string.Format("@{0} {1}", tweetToReplyTo.CreatedBy.ScreenName, text);
            var tweet = Tweet.PublishTweetInReplyTo(textToPublish, tweetIdtoReplyTo);
            Console.WriteLine("Publish success? {0}", tweet != null);
        }

        public static void Tweet_PublishTweetWithGeo(string text)
        {
            const double longitude = -122.400612831116;
            const double latitude = 37.7821120598956;

            var publishParameters = new PublishTweetOptionalParameters();
            publishParameters.Coordinates = new Coordinates(longitude, latitude);

            var tweet = Tweet.PublishTweet(text, publishParameters);

            Console.WriteLine(tweet.IsTweetPublished);
        }

        public static void Tweet_PublishRetweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            var retweet = tweet.PublishRetweet();

            Console.WriteLine("You retweeted : '{0}'", retweet.Text);
        }

        public static void Tweet_DestroyRetweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            var retweet = tweet.PublishRetweet();

            retweet.Destroy();
        }

        public static void Tweet_GetRetweets(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            IEnumerable<ITweet> retweets = tweet.GetRetweets();

            var firstRetweeter = retweets.ElementAt(0).CreatedBy;
            var originalTweet = retweets.ElementAt(0).RetweetedTweet;
            Console.WriteLine("{0} retweeted : '{1}'", firstRetweeter.Name, originalTweet.Text);
        }

        public static void Tweet_GenerateOEmbedTweet()
        {
            var newTweet = Tweet.PublishTweet("to be oembed");

            var oembedTweet = newTweet.GenerateOEmbedTweet();

            Console.WriteLine("Oembed tweet url : {0}", oembedTweet.URL);

            if (newTweet.IsTweetPublished)
            {
                newTweet.Destroy();
            }
        }

        public static void Tweet_Destroy()
        {
            var newTweet = Tweet.PublishTweet("to be destroyed!");
            bool isTweetPublished = newTweet.IsTweetPublished;

            if (isTweetPublished)
            {
                newTweet.Destroy();
            }

            bool tweetDestroyed = newTweet.IsTweetDestroyed;
            Console.WriteLine("Has the tweet destroyed? {0}", tweetDestroyed);
        }

        public static void Tweet_SetTweetAsFavorite(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            tweet.Favorite();
            Console.WriteLine("Is tweet now favourite? -> {0}", tweet.Favorited);
        }

        #endregion

        #region User

        public static void User_GetCurrentUser()
        {
            var user = User.GetAuthenticatedUser();
            Console.WriteLine(user.ScreenName);
        }

        public static void User_GetUserFromId(long userId)
        {
            var user = User.GetUserFromId(userId);
            Console.WriteLine(user.ScreenName);
        }

        public static void User_GetUserFromName(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            Console.WriteLine(user.Id);
        }

        public static void User_GetFriendIds(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var friendIds = user.GetFriendIds();

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friendId in friendIds)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        public static void User_GetFriendIdsUpTo(string userName, int limit)
        {
            var user = User.GetUserFromScreenName(userName);
            var friendIds = user.GetFriendIds(limit);

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friendId in friendIds)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        public static void User_GetFriends(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var friends = user.GetFriends();

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friend in friends)
            {
                Console.WriteLine("- {0}", friend.Name);
            }
        }

        public static void User_GetFollowerIds(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var followerIds = user.GetFollowerIds();

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var followerId in followerIds)
            {
                Console.WriteLine("- {0}", followerId);
            }
        }

        public static void User_GetFollowerIdsUpTo(string userName, int limit)
        {
            var user = User.GetUserFromScreenName(userName);
            var followerIds = user.GetFollowerIds(limit);

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var followerId in followerIds)
            {
                Console.WriteLine("- {0}", followerId);
            }
        }

        public static void User_GetFollowers(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var followers = user.GetFollowers();

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var follower in followers)
            {
                Console.WriteLine("- {0}", follower.Name);
            }
        }

        public static void User_GetRelationshipBetween(string sourceUserName, string targetUsername)
        {
            var sourceUser = User.GetUserFromScreenName(sourceUserName);
            var targetUser = User.GetUserFromScreenName(targetUsername);

            var relationship = sourceUser.GetRelationshipWith(targetUser);
            Console.WriteLine("You are{0} following {1}", relationship.Following ? "" : " not", targetUsername);
            Console.WriteLine("You are{0} being followed by {1}", relationship.FollowedBy ? "" : " not", targetUsername);
        }

        public static void User_GetFavorites(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var favorites = user.GetFavorites();

            Console.WriteLine("{0} has {1} favorites, here are some of them :", user.Name, user.FavouritesCount);
            foreach (var favoriteTweet in favorites)
            {
                Console.WriteLine("- {0}", favoriteTweet.Text);
            }
        }

        public static void User_BlockUser(string userName)
        {
            var user = User.GetUserFromScreenName(userName);

            if (user.BlockUser())
            {
                Console.WriteLine("{0} has been blocked.", userName);
            }
            else
            {
                Console.WriteLine("{0} has not been blocked.", userName);
            }
        }

        public static void User_UnBlockUser(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            user.UnBlockUser();
        }

        public static void User_GetBlockedUsers()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            authenticatedUser.GetBlockedUsers();
            authenticatedUser.GetBlockedUserIds();
        }

        public static void User_DownloadProfileImage(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var stream = user.GetProfileImageStream(ImageSize.bigger);
            var fileStream = new FileStream(string.Format("{0}.jpg", user.Id), FileMode.Create);
            stream.CopyTo(fileStream);

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (assemblyPath != null)
            {
                Process.Start(assemblyPath);
            }
        }

        public static void User_GetMutedUsers()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            authenticatedUser.GetMutedUserIds();
        }

        #endregion

        #region AuthenticatedUser

        public static void AuthenticatedUser_GetIncomingRequests()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var usersRequestingFriendship = authenticatedUser.GetUsersRequestingFriendship();

            foreach (var user in usersRequestingFriendship)
            {
                Console.WriteLine("{0} wants to follow you!", user.Name);
            }
        }

        public static void AuthenticatedUser_GetOutgoingRequests()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var usersRequestingFriendship = authenticatedUser.GetUsersYouRequestedToFollow();

            foreach (var user in usersRequestingFriendship)
            {
                Console.WriteLine("You sent a request to follow {0}!", user.Name);
            }
        }

        public static void AuthenticatedUser_FollowUser(string userName)
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (authenticatedUser.FollowUser(userToFollow))
            {
                Console.WriteLine("You have successfully sent a request to follow {0}", userToFollow.Name);
            }
        }

        public static void AuthenticatedUser_UnFollowUser(string userName)
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (authenticatedUser.UnFollowUser(userToFollow))
            {
                Console.WriteLine("You are not following {0} anymore", userToFollow.Name);
            }
        }

        public static void AuthenticatedUser_UpdateFollowAuthorizationsForUser(string userName)
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (authenticatedUser.UpdateRelationshipAuthorizationsWith(userToFollow, false, false))
            {
                Console.WriteLine("Authorizations updated");
            }
        }

        public static void AuthenticatedUser_GetLatestReceivedMessages()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var messages = authenticatedUser.GetLatestMessagesReceived(20);

            Console.WriteLine("Messages Received : ");
            foreach (var message in messages)
            {
                Console.WriteLine("- '{0}'", message.Text);
            }
        }

        public static void AuthenticatedUser_GetLatestSentMessages()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var messages = authenticatedUser.GetLatestMessagesSent(20);

            Console.WriteLine("Messages Received : ");
            foreach (var message in messages)
            {
                Console.WriteLine("- '{0}'", message.Text);
            }
        }

        public static void AuthenticatedUser_GetAccountSettings()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var settings = authenticatedUser.GetAccountSettings();

            // Store information
            authenticatedUser.AccountSettings = settings;

            Console.WriteLine("{0} uses lang : {1}", settings.ScreenName, settings.Language);
        }

        #endregion

        #region Timeline

        public static void Timeline_GetUserTimeline(string username)
        {
            var user = User.GetUserFromScreenName(username);

            var timelineTweets = user.GetUserTimeline();
            foreach (var tweet in timelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        public static void Timeline_GetHomeTimeline()
        {
            var authenticatedUser = User.GetAuthenticatedUser();

            var homeTimelineTweets = authenticatedUser.GetHomeTimeline();
            foreach (var tweet in homeTimelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        public static void Timeline_GetMentionsTimeline()
        {
            var authenticatedUser = User.GetAuthenticatedUser();

            var mentionsTimelineTweets = authenticatedUser.GetMentionsTimeline();
            foreach (var mention in mentionsTimelineTweets)
            {
                Console.WriteLine(mention.Text);
            }
        }

        #endregion

        #region Stream

        public static void SimpleStream_Events()
        {
            var stream = Stream.CreateTweetStream();
            stream.TweetReceived += (sender, args) => { };
            stream.TweetDeleted += (sender, args) => { };
            stream.TweetLocationInfoRemoved += (sender, args) => { };

            stream.TweetWitheld += (sender, args) => { };
            stream.UserWitheld += (sender, args) => { };

            stream.LimitReached += (sender, args) => { };
            stream.DisconnectMessageReceived += (sender, args) => { };
            stream.WarningFallingBehindDetected += (sender, args) => { };

            stream.UnmanagedEventReceived += (sender, args) => { };
        }

        public static void Stream_SampleStreamExample()
        {
            var stream = Stream.CreateSampleStream();

            stream.TweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet.Text);
            };

            stream.AddTweetLanguageFilter(Language.English);
            stream.AddTweetLanguageFilter(Language.French);

            stream.StartStream();
        }

        public static void Stream_FilteredStreamExample()
        {
            var stream = Stream.CreateFilteredStream();
            var location = new Location(-124.75, 36.8, -126.89, 32.75);

            stream.AddLocation(location);
            stream.AddTrack("tweetinvi");
            stream.AddTrack("linvi");

            stream.MatchingTweetReceived += (sender, args) =>
            {
                var tweet = args.Tweet;
                Console.WriteLine("{0} was detected between the following tracked locations:", tweet.Id);

                IEnumerable<ILocation> matchingLocations = args.MatchingLocations;
                foreach (var matchingLocation in matchingLocations)
                {
                    Console.Write("({0}, {1}) ;", matchingLocation.Coordinate1.Latitude, matchingLocation.Coordinate1.Longitude);
                    Console.WriteLine("({0}, {1})", matchingLocation.Coordinate2.Latitude, matchingLocation.Coordinate2.Longitude);
                }
            };

            stream.StartStreamMatchingAllConditions();
        }

        public static void Stream_UserStreamExample()
        {
            var userStream = Stream.CreateUserStream();

            // Tweet Published
            EventsRelatedWithTweetCreation(userStream);

            // Messages
            EventsRelatedWithMessages(userStream);

            // Favourited - Unfavourited
            EventsRelatedWithTweetAndFavourite(userStream);

            // Lists
            EventsRelatedWithLists(userStream);

            // Block - Unblock
            EventsRelatedWithBlock(userStream);

            // User Update
            userStream.AuthenticatedUserProfileUpdated += (sender, args) =>
            {
                var newAuthenticatedUser = args.AuthenticatedUser;
                Console.WriteLine("Authenticated user '{0}' has been updated!", newAuthenticatedUser.Name);
            };

            // Friends the stream will analyze - A UserStream cannot analyze more than 10.000 people at the same time
            userStream.FriendIdsReceived += (sender, args) =>
            {
                var friendIds = args.Value;
                Console.WriteLine(friendIds.Count());
            };

            // Access Revoked
            userStream.AccessRevoked += (sender, args) =>
            {
                Console.WriteLine("Application {0} had its access revoked!", args.Info.ApplicationName);
            };

            userStream.StartStream();
        }

        public static void EventsRelatedWithTweetCreation(IUserStream userStream)
        {
            userStream.TweetCreatedByAnyone += (sender, args) =>
            {
                Console.WriteLine("Tweet created by anyone");
            };

            userStream.TweetCreatedByMe += (sender, args) =>
            {
                Console.WriteLine("Tweet created by me");
            };

            userStream.TweetCreatedByAnyoneButMe += (sender, args) =>
            {
                Console.WriteLine("Tweet created by {0}", args.Tweet.CreatedBy.Name);
            };
        }

        public static void EventsRelatedWithMessages(IUserStream userStream)
        {
            userStream.MessageSent += (sender, args) => { Console.WriteLine("message '{0}' sent", args.Message.Text); };
            userStream.MessageReceived += (sender, args) => { Console.WriteLine("message '{0}' received", args.Message.Text); };
        }

        public static void EventsRelatedWithTweetAndFavourite(IUserStream userStream)
        {
            // Favourite
            userStream.TweetFavouritedByAnyone += (sender, args) =>
            {
                var tweet = args.Tweet;
                var userWhoFavouritedTheTweet = args.FavouritingUser;
                Console.WriteLine("User '{0}' favourited tweet '{1}'", userWhoFavouritedTheTweet.Name, tweet.Id);
            };

            userStream.TweetFavouritedByMe += (sender, args) =>
            {
                var tweet = args.Tweet;
                var authenticatedUser = args.FavouritingUser;
                Console.WriteLine("Authenticated User '{0}' favourited tweet '{1}'", authenticatedUser.Name, tweet.Id);
            };

            userStream.TweetFavouritedByAnyoneButMe += (sender, args) =>
            {
                var tweet = args.Tweet;
                var userWhoFavouritedTheTweet = args.FavouritingUser;
                Console.WriteLine("User '{0}' favourited tweet '{1}'", userWhoFavouritedTheTweet.Name, tweet.Id);
            };

            // Unfavourite
            userStream.TweetUnFavouritedByAnyone += (sender, args) =>
            {
                var tweet = args.Tweet;
                var userWhoFavouritedTheTweet = args.FavouritingUser;
                Console.WriteLine("User '{0}' unfavourited tweet '{1}'", userWhoFavouritedTheTweet.Name, tweet.Id);
            };

            userStream.TweetUnFavouritedByMe += (sender, args) =>
            {
                Console.WriteLine("Tweet unfavourited by me!");
            };

            userStream.TweetUnFavouritedByAnyoneButMe += (sender, args) =>
            {
                var tweet = args.Tweet;
                var userWhoFavouritedTheTweet = args.FavouritingUser;
                Console.WriteLine("User '{0}' favourited tweet '{1}'", userWhoFavouritedTheTweet.Name, tweet.Id);
            };
        }

        public static void EventsRelatedWithLists(IUserStream userStream)
        {
            userStream.ListCreated += (sender, args) =>
            {
                Console.WriteLine("List '{0}' created!", args.List.Name);
            };

            userStream.ListUpdated += (sender, args) =>
            {
                Console.WriteLine("List '{0}' updated!", args.List.Name);
            };

            userStream.ListDestroyed += (sender, args) =>
            {
                Console.WriteLine("List '{0}' destroyed!", args.List.Name);
            };

            // User Added
            userStream.AuthenticatedUserAddedMemberToList += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You added '{0}' to the list : '{1}'", newUser.Name, list.Name);
            };

            userStream.AuthenticatedUserAddedToListBy += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You haved been added to the list '{0}' by '{1}'", list.Name, newUser.Name);
            };

            // User Removed
            userStream.AuthenticatedUserRemovedMemberFromList += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You removed '{0}' from the list : '{1}'", newUser.Name, list.Name);
            };

            userStream.AuthenticatedUserRemovedFromListBy += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You haved been removed from the list '{0}' by '{1}'", list.Name, newUser.Name);
            };

            // User Subscribed
            userStream.AuthenticatedUserSubscribedToListCreatedBy += (sender, args) =>
            {
                var list = args.List;
                Console.WriteLine("You have subscribed to the list '{0}", list.Name);
            };

            userStream.UserSubscribedToListCreatedByMe += (sender, args) =>
            {
                var list = args.List;
                var user = args.User;
                Console.WriteLine("'{0}' have subscribed to your list '{1}'", user.Name, list.Name);
            };

            // User Unsubscribed
            userStream.AuthenticatedUserUnsubscribedToListCreatedBy += (sender, args) =>
            {
                var list = args.List;
                Console.WriteLine("You have unsubscribed from the list '{0}'", list.Name);
            };

            userStream.UserUnsubscribedToListCreatedByMe += (sender, args) =>
            {
                var list = args.List;
                var user = args.User;
                Console.WriteLine("'{0}' have unsubscribed from your list '{1}'", user.Name, list.Name);
            };
        }

        public static void EventsRelatedWithBlock(IUserStream userStream)
        {
            userStream.BlockedUser += (sender, args) =>
            {
                Console.WriteLine("I blocked a '{0}'", args.User.ScreenName);
            };

            userStream.UnBlockedUser += (sender, args) =>
            {
                Console.WriteLine("I un blocked a '{0}'", args.User.ScreenName);
            };
        }

        #endregion

        #region Search

        public static void Search_SimpleTweetSearch()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!
            var tweets = Search.SearchTweets("#obama");

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}", tweet.Text);
            }
        }

        public static void Search_SearchTweet()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!

            var searchParameter = Search.CreateTweetSearchParameter("obama");

            searchParameter.SetGeoCode(new Coordinates(-122.398720, 37.781157), 1, DistanceMeasure.Miles);
            searchParameter.Lang = Language.English;
            searchParameter.SearchType = SearchResultType.Popular;
            searchParameter.MaximumNumberOfResults = 100;
            searchParameter.Since = new DateTime(2013, 12, 1);
            searchParameter.Until = new DateTime(2013, 12, 11);
            searchParameter.SinceId = 399616835892781056;
            searchParameter.MaxId = 405001488843284480;
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
            searchParameter.Filters = TweetSearchFilters.Videos;

            var tweets = Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static void Search_SearchWithMetadata()
        {
            Search.SearchTweetsWithMetadata("hello");
        }

        public static void Search_FilteredSearch()
        {
            var searchParameter = Search.CreateTweetSearchParameter("#tweetinvi");
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;

            var tweets = Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static void Search_SearchAndGetMoreThan100Results()
        {
            var searchParameter = Search.CreateTweetSearchParameter("us");
            searchParameter.MaximumNumberOfResults = 200;

            var tweets = Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static void Search_SearchUsers()
        {
            var users = Search.SearchUsers("linvi", 100);
            users.ForEach(Console.WriteLine);
        }

        #endregion

        #region Saved Searches

        public static void SavedSearch_GetSavedSearches()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var savedSearches = authenticatedUser.GetSavedSearches();

            Console.WriteLine("Saved Searches");
            foreach (var savedSearch in savedSearches)
            {
                Console.WriteLine("- {0} => {1}", savedSearch.Name, savedSearch.Query);
            }
        }

        public static void SavedSearch_CreateSavedSearch(string query)
        {
            var savedSearch = SavedSearch.CreateSavedSearch(query);
            Console.WriteLine("Saved Search created as : {0}", savedSearch.Name);
        }

        public static void SavedSearch_GetSavedSearch(long searchId)
        {
            var savedSearch = SavedSearch.GetSavedSearch(searchId);
            Console.WriteLine("Saved searched query is : '{0}'", savedSearch.Query);
        }

        public static void SavedSearch_DestroySavedSearch(long searchId)
        {
            var savedSearch = SavedSearch.GetSavedSearch(searchId);

            if (SavedSearch.DestroySavedSearch(savedSearch))
            {
                Console.WriteLine("You destroyed the search successfully!");
            }
        }

        #endregion

        #region Message

        public static void Message_GetLatests()
        {
            // Messages Received
            var latestMessagesReceived = Message.GetLatestMessagesReceived();
            var latestMessagesReceivedParameter = new MessagesReceivedParameters();
            latestMessagesReceivedParameter.SinceId = 10029230923;
            var latestMessagesReceivedFromParameter = Message.GetLatestMessagesReceived(latestMessagesReceivedParameter);

            // Messages Sent
            var latestMessagesSent = Message.GetLatestMessagesSent();
            var latestMessagesSentParameter = new MessagesSentParameters();
            latestMessagesSentParameter.PageNumber = 239823;
            var latestMessagesSentFromParameter = Message.GetLatestMessagesSent(latestMessagesSentParameter);
        }

        public static void Message_GetMessageFromId(long messageId)
        {
            var message = Message.GetExistingMessage(messageId);
            Console.WriteLine("Message from {0} to {1} : {2}", message.Sender, message.Recipient, message.Text);
        }

        public static void Message_DestroyMessageFromId(long messageId)
        {
            var message = Message.GetExistingMessage(messageId);
            if (message.Destroy())
            {
                Console.WriteLine("Message successfully destroyed!");
            }
        }

        public static void Message_PublishMessage(string text, string username)
        {
            var recipient = User.GetUserFromScreenName(username);
            var message = Message.PublishMessage(text, recipient);

            if (message != null)
            {
                Console.WriteLine("Message published : {0}", message.IsMessagePublished);
            }
        }

        #endregion

        #region Lists

        public static void TwitterList_GetUserOwnedLists()
        {
            var user = User.GetAuthenticatedUser();
            var ownedLists = TwitterList.GetUserOwnedLists(user);

            ownedLists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        public static void TwitterList_GetUserSubscribedLists()
        {
            var currentUser = User.GetAuthenticatedUser();
            var lists = TwitterList.GetUserSubscribedLists(currentUser);

            lists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        public static void TwitterList_GetExistingListById(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            Console.WriteLine("You have retrieved the list {0}", list.Name);
        }

        public static void TwitterList_CreateList()
        {
            var list = TwitterList.CreateList("plop", PrivacyMode.Public, "description");
            Console.WriteLine("List '{0}' has been created!", list.FullName);
        }

        public static void TwitterList_UpdateList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var updateParameters = new TwitterListUpdateParameters();
            updateParameters.Name = "piloupe";
            updateParameters.Description = "pilouping description";
            updateParameters.PrivacyMode = PrivacyMode.Private;
            list.Update(updateParameters);

            Console.WriteLine("List new name is : {0}", list.Name);
        }

        public static void TwitterList_DestroyList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var hasBeenDestroyed = list.Destroy();
            Console.WriteLine("Tweet {0} been destroyed.", hasBeenDestroyed ? "has" : "has not");
        }

        public static void TwitterList_GetTweetsFromList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var tweets = list.GetTweets();

            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        public static void TwitterList_GetMembersOfList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var members = list.GetMembers();

            members.ForEach(x => Console.WriteLine(x.Name));
        }

        public static void TwitterList_CheckUserMembership(long userId, long listId)
        {
            var isUserAMember = TwitterList.CheckIfUserIsAListMember(userId, listId);
            Console.WriteLine("{0} is{1}a member of list {2}", userId, isUserAMember ? " " : " NOT ", listId);
        }

        public static void TwitterList_GetSubscribers(long listId)
        {
            var subscribers = TwitterList.GetListSubscribers(listId);

            subscribers.ForEach(user => Console.WriteLine(user));
        }

        public static void TwitterList_SubscribeOrUnsubscribeToList(long listId)
        {
            var hasSuccessfullySubscribed = TwitterList.SubscribeAuthenticatedUserToList(listId);
            var hasUnsubscribed = TwitterList.UnSubscribeAuthenticatedUserToList(listId);
        }

        public static void TwitterList_CheckUserSubscription(long userId, long listId)
        {
            var isUserASubscriber = TwitterList.CheckIfUserIsAListSubscriber(userId, listId);
            Console.WriteLine("{0} is{1}subscribed to the list {2}", userId, isUserASubscriber ? " " : " NOT ", listId);
        }

        #endregion

        #region Geo/Trends

        public static void Geo_GetPlaceFromId(string placeId)
        {
            var geoController = TweetinviContainer.Resolve<IGeoController>();
            var place = geoController.GetPlaceFromId(placeId);
            Console.WriteLine(place.Name);
        }

        public static void Trends_GetTrendsFromWoeId(long woeid)
        {
            var trendsController = TweetinviContainer.Resolve<ITrendsController>();
            var placeTrends = trendsController.GetPlaceTrendsAt(woeid);
            Console.WriteLine(placeTrends.woeIdLocations.First().Name);
        }

        #endregion

        #region Rate Limits

        public static void RateLimits_Track_Examples()
        {
            // Enable Tweetinvi RateLimit Handler
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            // Get notified when your application is being stopped to wait for RateLimits to be available
            RateLimit.QueryAwaitingForRateLimit += (sender, args) =>
            {
                Console.WriteLine("{0} is awaiting {1}ms for RateLimit to be available", args.Query, args.ResetInMilliseconds);
            };

            // Get the RateLimit associated with a query, this can return null
            var queryRateLimit = RateLimit.GetQueryRateLimit("https://api.twitter.com/1.1/application/rate_limit_status.json");

            // Pause the current thread until the specific RateLimit can be used
            RateLimit.AwaitForQueryRateLimit(queryRateLimit);

            // This stop the execution of the current thread until the RateLimits are available 
            RateLimit.AwaitForQueryRateLimit("https://api.twitter.com/1.1/application/rate_limit_status.json");

            // Tweetinvi uses a cache mechanism to store credentials RateLimits, this operation allows you to clear it.
            // If the UseRateLimitAwaiter option is enabled this will result in the RateLimits to be retrieved during the next query.
            RateLimit.ClearRateLimitCache();
        }

        public static void RateLimits_ManualAwait()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                var queryRateLimit = RateLimit.GetQueryRateLimit(args.QueryURL);
                RateLimit.AwaitForQueryRateLimit(queryRateLimit);
            };
        }

        public static void GetCurrentCredentialsRateLimits()
        {
            var tokenRateLimits = RateLimit.GetCurrentCredentialsRateLimits();

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToLongTimeString());
        }

        public static void GetCredentialsRateLimits()
        {
            var credentials = Auth.CreateCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var tokenRateLimits = RateLimit.GetCredentialsRateLimits(credentials);

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToLongTimeString());
        }

        #endregion

        #region Account & Relationships

        public static void Account_GetAndSetAccountSettings()
        {
            var settings = Account.GetCurrentAccountSettings();

            var updatedSettingsRequestParameter = Account.CreateUpdateAccountSettingsRequestParameters(settings);
            updatedSettingsRequestParameter.SleepTimeEnabled = false;

            var updatedSettings = Account.UpdateAccountSettings(updatedSettingsRequestParameter);
            Console.WriteLine(updatedSettings.SleepTimeEnabled);
        }

        public static void Account_Mute()
        {
            // Get
            Account.GetMutedUserIds();
            Account.GetMutedUsers();

            // Create
            Account.MuteUser("username");

            // Delete
            Account.UnMuteUser("username");
        }

        public static void Friendship_GetMultipleRelationships()
        {
            var user1 = User.GetUserFromScreenName("tweetinviapi");
            var user2 = User.GetUserFromScreenName(USER_SCREEN_NAME_TO_TEST);

            var userList = new List<IUser>
            {
                user1,
                user2
            };

            var relationships = Account.GetMultipleRelationships(userList);
            foreach (var relationship in relationships)
            {
                Console.WriteLine("You are{0} following {1}", relationship.Following ? "" : " not", relationship.TargetScreenName);
                Console.WriteLine("You are{0} being followed by {1}", relationship.FollowedBy ? "" : " not", relationship.TargetScreenName);
                Console.WriteLine();
            }
        }

        public static void Friendship_GetUserRequestingFriendship()
        {
            var friendshipRequests = Account.GetUsersRequestingFriendship();

            foreach (var friendshipRequest in friendshipRequests)
            {
                Console.WriteLine("{0} requested to be your friend!", friendshipRequest.Name);
            }
        }

        public static void Friendship_GetUsersYouRequestedToFollow()
        {
            var usersYouWantToFollow = Account.GetUsersYouRequestedToFollow();

            foreach (var user in usersYouWantToFollow)
            {
                Console.WriteLine("You want to be friend with {0}.", user.Name);
            }
        }

        public static void Friendship_GetUsersWhoCantRetweet()
        {
            Account.GetUsersWhoseRetweetsAreMuted();
        }

        public static void Friendship_UpdateRelationship()
        {
            const bool enableRetweets = true;
            const bool enableNotificationsOnDevices = true;

            var success = Account.UpdateRelationshipAuthorizationsWith("tweetinviapi", enableRetweets, enableNotificationsOnDevices);
        }

        public static void Friendship_GetRelationshipDetails()
        {
            var relationshipDetails = Friendship.GetRelationshipDetailsBetween("tweetinviapi", "twitterapi");
        }

        #endregion

        #region Upload

        public static IMedia ChunkedUpload(byte[] binary, string mediaType)
        {
            var uploader = Upload.CreateChunkedUploader();
            var half = (binary.Length / 2);
            var first = binary.Take(half).ToArray();
            var second = binary.Skip(half).ToArray();

            if (uploader.Init(mediaType, binary.Length))
            {
                if (uploader.Append(first, "media"))
                {
                    if (uploader.Append(second, "media"))
                    {
                        return uploader.Complete();
                    }
                }
            }

            return null;
        }

        #endregion

        #region Json

        public static void Json_GetJsonForAccountRequestExample()
        {
            string jsonResponse = AccountJson.GetAuthenticatedUserSettingsJson();
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForMessageRequestExample()
        {
            IUser user = User.GetUserFromScreenName("tweetinviapi");
            string jsonResponse = MessageJson.PublishMessage("salut", user.UserDTO);

            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForGeoRequestExample()
        {
            var jsonResponse = GeoJson.GetPlaceFromId("df51dec6f4ee2b2c");
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForHelpRequestExample()
        {
            var jsonResponse = HelpJson.GetCredentialsRateLimits();
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForSavedSearchRequestExample()
        {
            var jsonResponse = SavedSearchJson.GetSavedSearches();
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForTimelineRequestExample()
        {
            var jsonResponse = TimelineJson.GetHomeTimeline(2);
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForTrendsRequestExample()
        {
            var jsonResponse = TrendsJson.GetTrendsAt(1);
            Console.WriteLine(jsonResponse);
        }

        public static void Json_GetJsonForTweetRequestExample()
        {
            var json = TweetJson.PublishTweet("text");
        }

        public static void Json_GetJsonForUserRequestExample()
        {
            var authenticatedUser = User.GetAuthenticatedUser();
            var jsonResponse = UserJson.GetFriendIds(authenticatedUser);
            Console.WriteLine(jsonResponse.ElementAt(0));
        }

        public static void Json_GetJsonCursorRequestExample()
        {
            // This query is a cursor query
            var jsonResponses = FriendshipJson.GetUserIdsRequestingFriendship();

            foreach (var jsonResponse in jsonResponses)
            {
                Console.WriteLine(jsonResponse);
            }
        }

        #endregion

        #region Exception

        public static void Exceptions_GetExceptionsInfo()
        {
            Auth.Credentials = null;

            var user = User.GetAuthenticatedUser();
            if (user == null)
            {
                var lastException = ExceptionHandler.GetLastException();
                Console.WriteLine(lastException.TwitterDescription);
            }
        }

        #endregion

        #region Manual Query

        public static void ManualQuery_Example()
        {
            const string getHomeTimelineQuery = "https://api.twitter.com/1.1/statuses/home_timeline.json";

            // Execute Query can either return a json or a DTO interface
            var tweetsDTO = TwitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(getHomeTimelineQuery);
            tweetsDTO.ForEach(tweetDTO => Console.WriteLine(tweetDTO.Text));
        }

        #endregion

        #region Configuration (Timeout/Proxy)

        public static void ConfigureTweetinvi()
        {
            if (!ExecuteExamples)
            {
                return;
            }

            TweetinviConfig.CurrentThreadSettings.ProxyURL = "http://228.23.13.21:4287";

            // Configure a proxy with Proxy with username and password
            TweetinviConfig.CurrentThreadSettings.ProxyURL = "http://user:pass@228.23.13.21:4287";

            TweetinviConfig.CurrentThreadSettings.HttpRequestTimeout = 5000;

            TweetinviConfig.CurrentThreadSettings.UploadTimeout = 90000;
        }

        public static void GlobalEvents()
        {
            if (!ExecuteExamples)
            {
                return;
            }

            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} is about to be executed.", args.TwitterQuery);
            };

            TweetinviEvents.QueryAfterExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} has just been executed.", args.TwitterQuery);
            };

            TweetinviEvents.CurrentThreadEvents.QueryBeforeExecute += (sender, args) =>
            {
                Console.WriteLine("The query {0} is about to be executed in the main Thread.", args.TwitterQuery);
            };
        }

        #endregion

        #region Help

        public static void GetTwitterPrivacyPolicy()
        {
            Console.WriteLine(Help.GetTwitterPrivacyPolicy());
        }

        #endregion
    }
}