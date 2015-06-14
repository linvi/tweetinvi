using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Json;
using Stream = Tweetinvi.Stream;

namespace Examplinvi
{
    // IMPORTANT 
    // This cheat sheet provide examples for all the features provided by Tweetinvi.

    // WINDOWS PHONE 8 developers
    // If you are a windows phone developer, please use the Async classes
    // User.GetLoggedUser(); -> await UserAsync.GetLoggedUser();

    class Program
    {
        private const string USER_SCREEN_NAME_TO_TEST = "ladygaga";

        static void Main()
        {
            TwitterCredentials.SetCredentials("Access_Token", "Access_Token_Secret", "Consumer_Key", "Consumer_Secret");

            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                // Console.WriteLine(args.QueryURL);
            };

            GenerateCredentialExamples();
            UserLiveFeedExamples();
            TweetExamples();
            UserExamples();
            LoggedUserExamples();
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
            OtherFeaturesExamples();
            ConfigureTweetinvi();
            GlobalEvents();

            Console.WriteLine(@"END");
            Console.ReadLine();
        }

        #region Examples Store

        // ReSharper disable LocalizableElement
        // ReSharper disable UnusedMember.Local

        private static void GenerateCredentialExamples()
        {
            // With captcha
            //CredentialsCreator_WithCaptcha_StepByStep("CONSUMER_KEY", "CONSUMER_SECRET");
            //CredentialsCreator_WithCaptcha_SingleStep(ConfigurationManager.AppSettings["token_ConsumerKey"], ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            // With callback URL
            //CredentialsCreator_CreateFromRedirectedCallbackURL_StepByStep(ConfigurationManager.AppSettings["token_ConsumerKey"], ConfigurationManager.AppSettings["token_ConsumerSecret"]);
            //CredentialsCreator_CreateFromRedirectedCallbackURL_SingleStep(ConfigurationManager.AppSettings["token_ConsumerKey"], ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            //CredentialsCreator_CreateFromRedirectedVerifierCode_StepByStep(ConfigurationManager.AppSettings["token_ConsumerKey"], ConfigurationManager.AppSettings["token_ConsumerSecret"]);
            //CredentialsCreator_CreateFromRedirectedVerifierCode_SingleStep(ConfigurationManager.AppSettings["token_ConsumerKey"], ConfigurationManager.AppSettings["token_ConsumerSecret"]);
        }

        private static void UserLiveFeedExamples()
        {
            // Stream_UserStreamExample();
        }

        private static void TweetExamples()
        {
            // Tweet_GetExistingTweet(210462857140252672);

            // Tweet_PublishTweet(String.Format("I love tweetinvi! ({0})", Guid.NewGuid()));
            // Tweet_PublishTweetWithImage("Uploadinvi?", "YOUR_FILE_PATH.png");

            // Tweet_PublishTweetInReplyToAnotherTweet(String.Format("I love tweetinvi! ({0})", Guid.NewGuid()), 392711547081854976);
            // Tweet_PublishTweetWithGeo(String.Format("I love tweetinvi! ({0})", Guid.NewGuid()));
            // Tweet_PublishTweetWithGeo_Coordinates(String.Format("I love tweetinvi! ({0})", Guid.NewGuid()));

            // Tweet_Destroy();

            // Tweet_GetRetweets(210462857140252672);
            // Tweet_PublishRetweet(210462857140252672);

            // Tweet_GenerateOEmbedTweet();

            // Tweet_DestroyRetweet(210462857140252672);

            // Tweet_SetTweetAsFavorite(392711547081854976);
        }

        private static void UserExamples()
        {
            // User_GetCurrentUser();

            // User_GetUserFromName(1478171);
            // User_GetUserFromName(USER_SCREEN_NAME_TO_TEST);

            // User_GetFavorites(USER_SCREEN_NAME_TO_TEST);

            // User_GetFriends(USER_SCREEN_NAME_TO_TEST);
            // User_GetFriendIds(USER_SCREEN_NAME_TO_TEST);
            // User_GetFriendIdsUpTo(USER_SCREEN_NAME_TO_TEST, 10000);

            // User_GetFollowers(USER_SCREEN_NAME_TO_TEST);
            // User_GetFollowerIds(USER_SCREEN_NAME_TO_TEST);
            // User_GetFollowerIdsUpTo(USER_SCREEN_NAME_TO_TEST, 10000);

            // User_GetRelationshipBetween("tweetinvitest", USER_SCREEN_NAME_TO_TEST);

            // User_BlockUser(USER_SCREEN_NAME_TO_TEST);
            // User_UnBlockUser(USER_SCREEN_NAME_TO_TEST);
            // User_GetBlockedUsers();
            // User_DownloadProfileImage(USER_SCREEN_NAME_TO_TEST);
            // User_DownloadProfileImageAsync(USER_SCREEN_NAME_TO_TEST);
            // User_GenerateProfileImageStream(USER_SCREEN_NAME_TO_TEST);
            // User_GenerateProfileImageBitmap(USER_SCREEN_NAME_TO_TEST);
        }

        private static void LoggedUserExamples()
        {
            //LoggedUser_GetMultipleRelationships();
            //LoggedUser_GetIncomingRequests();
            //LoggedUser_GetOutgoingRequests();
            //LoggedUser_FollowUser(USER_SCREEN_NAME_TO_TEST);
            //LoggedUser_UnFollowUser(USER_SCREEN_NAME_TO_TEST);
            //LoggedUser_UpdateFollowAuthorizationsForUser(USER_SCREEN_NAME_TO_TEST);
            //LoggedUser_GetLatestReceivedMessages();
            //LoggedUser_GetLatestSentMessages();
            //LoggedUser_GetAccountSettings();
        }

        private static void TimelineExamples()
        {
            // Timeline_GetHomeTimeline();
            // Timeline_GetUserTimeline(USER_SCREEN_NAME_TO_TEST);
            // Timeline_GetMentionsTimeline();
        }

        private static void MessageExamples()
        {
            // TokenUser_GetLatestReceivedMessages();
            // Message_GetMessageFromId(381069551028293633);
            // Message_PublishMessage(USER_SCREEN_NAME_TO_TEST);
            // Message_PublishMessageTo(USER_SCREEN_NAME_TO_TEST);
        }

        private static void TwitterListExamples()
        {
            // TwitterList_GetUserLists();
            // TwitterList_CreateList();
            // TwitterList_GetExistingListById(105601767);
            // TwitterList_UpdateList(105601767);
            // TwitterList_DestroyList(105601767);
            // TwitterList_GetTweetsFromList(105601767);
            // TwitterList_GetMembersOfList(105601767);
        }

        private static void GeoExamples()
        {
            // Geo_GetPlaceFromId("df51dec6f4ee2b2c");
            // Trends_GetTrendsFromWoeId(1);
        }

        private static void SearchExamples()
        {
            // Search_SimpleTweetSearch();
            // Search_SearchTweet();
            // Search_SearchWithMetadata();
            // Search_FilteredSearch();
            // Search_SearchUsers();
        }

        private static void SavedSearchesExamples()
        {
            // SavedSearch_CreateSavedSearch("tweetinvi");
            // SavedSearch_GetSavedSearches();
            // SavedSearch_GetSavedSearch(307102135);
            // SavedSearch_DestroySavedSearch(307102135);
        }

        private static void RateLimitExamples()
        {
            // RateLimits_Track_Examples();
            // GetCredentialsRateLimits();
            // GetCurrentCredentialsRateLimits();
        }

        private static void HelpExamples()
        {
            // GetTwitterPrivacyPolicy();
        }

        private static void JsonExamples()
        {
            // Json_GetJsonForAccountRequestExample();
            // Json_GetJsonForMessageRequestExample();
            // Json_GetJsonCursorRequestExample();
            // Json_GetJsonForGeoRequestExample();
            // Json_GetJsonForHelpRequestExample();
            // Json_GetJsonForSavedSearchRequestExample();
            // Json_GetJsonForTimelineRequestExample();
            // Json_GetJsonForTrendsRequestExample();
            // Json_GetJsonForTweetRequestExample();
            // Json_GetJsonForUserRequestExample();
        }

        private static void StreamExamples()
        {
            // Stream_SampleStreamExample();
            // Stream_FilteredStreamExample();
            // Stream_UserStreamExample();
            // SimpleStream_Events();
        }

        private static void OtherFeaturesExamples()
        {
            //Exceptions_GetExceptionsInfo();
            //ManualQuery_Example();
        }

        #endregion

        #region Credentials and Login

        // Get credentials with captcha system
        // ReSharper disable UnusedMethodReturnValue.Local
        private static IOAuthCredentials CredentialsCreator_WithCaptcha_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = CredentialsCreator.GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = CredentialsCreator.GetAuthorizationURL(applicationCredentials);
            Console.WriteLine("Go on : {0}", url);
            Console.WriteLine("Enter the captch : ");
            var captcha = Console.ReadLine();

            var newCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, applicationCredentials);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        private static IOAuthCredentials CredentialsCreator_WithCaptcha_SingleStep(string consumerKey, string consumerSecret)
        {
            Func<string, string> retrieveCaptcha = url =>
            {
                Console.WriteLine("Go on : {0}", url);
                Console.WriteLine("Enter the captch : ");
                return Console.ReadLine();
            };

            var newCredentials = CredentialsCreator.GetCredentialsFromCaptcha(retrieveCaptcha, consumerKey, consumerSecret);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        // Get credentials with callbackURL system
        private static IOAuthCredentials CredentialsCreator_CreateFromRedirectedCallbackURL_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = CredentialsCreator.GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = CredentialsCreator.GetAuthorizationURLForCallback(applicationCredentials, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Go on : {0}", url);
            Console.WriteLine("When redirected to your website copy and paste the URL: ");

            // Enter a value like: https://tweeetinvi.codeplex.com?oauth_token={tokenValue}&oauth_verifier={verifierValue}

            var callbackURL = Console.ReadLine();

            // Here we provide the entire URL where the user has been redirected
            var newCredentials = CredentialsCreator.GetCredentialsFromCallbackURL(callbackURL, applicationCredentials);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        private static IOAuthCredentials CredentialsCreator_CreateFromRedirectedVerifierCode_StepByStep(string consumerKey, string consumerSecret)
        {
            var applicationCredentials = CredentialsCreator.GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = CredentialsCreator.GetAuthorizationURLForCallback(applicationCredentials, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Go on : {0}", url);
            Console.WriteLine("When redirected to your website copy and paste the value of the oauth_verifier : ");

            // For the following redirection https://tweetinvi.codeplex.com?oauth_token=UR3eTEwDXFNhkMnjqz0oFbRauvAm4YhnF67KE6hO8Q&oauth_verifier=woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A
            // Enter the value : woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A

            var verifierCode = Console.ReadLine();

            // Here we only provide the verifier code
            var newCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(verifierCode, applicationCredentials);
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        private static IOAuthCredentials CredentialsCreator_CreateFromRedirectedCallbackURL_SingleStep(string consumerKey, string consumerSecret)
        {
            Func<string, string> retrieveCallbackURL = url =>
            {
                Console.WriteLine("Go on : {0}", url);
                Console.WriteLine("When redirected to your website copy and paste the URL: ");

                // Enter a value like: https://tweeetinvi.codeplex.com?oauth_token={tokenValue}&oauth_verifier={verifierValue}

                var callbackURL = Console.ReadLine();
                return callbackURL;
            };

            // Here we provide the entire URL where the user has been redirected
            var newCredentials = CredentialsCreator.GetCredentialsFromCallbackURL_UsingRedirectedCallbackURL(retrieveCallbackURL, consumerKey, consumerSecret, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }

        private static IOAuthCredentials CredentialsCreator_CreateFromRedirectedVerifierCode_SingleStep(string consumerKey, string consumerSecret)
        {
            Func<string, string> retrieveVerifierCode = url =>
            {
                Console.WriteLine("Go on : {0}", url);
                Console.WriteLine("When redirected to your website copy and paste the value of the oauth_verifier : ");

                // For the following redirection https://tweetinvi.codeplex.com?oauth_token=UR3eTEwDXFNhkMnjqz0oFbRauvAm4YhnF67KE6hO8Q&oauth_verifier=woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A
                // Enter the value : woXaKhpDtX6vhDVPl7TU6955qdQeH3cgz6xDvRZRA4A

                var verifierCode = Console.ReadLine();
                return verifierCode;
            };

            // Here we provide the entire URL where the user has been redirected
            var newCredentials = CredentialsCreator.GetCredentialsFromCallbackURL_UsingRedirectedVerifierCode(retrieveVerifierCode, consumerKey, consumerSecret, "https://tweetinvi.codeplex.com");
            Console.WriteLine("Access Token = {0}", newCredentials.AccessToken);
            Console.WriteLine("Access Token Secret = {0}", newCredentials.AccessTokenSecret);

            return newCredentials;
        }
        // ReSharper restore UnusedMethodReturnValue.Local


        #endregion

        #region Tweet

        private static void Tweet_GetExistingTweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            Console.WriteLine(tweet.Text);
        }

        private static void Tweet_PublishTweet(string text)
        {
            var newTweet = Tweet.CreateTweet(text);
            newTweet.Publish();

            Console.WriteLine(newTweet.IsTweetPublished);
        }

        private static void Tweet_PublishTweetWithImage(string text, string filePath, string filepath2 = null)
        {
            byte[] file1 = File.ReadAllBytes(filePath);

            // Create a tweet with a single image
            var tweet = Tweet.CreateTweetWithMedia(text, file1);

            // !! MOST ACCOUNTS ARE LIMITED TO 1 File per Tweet     !!
            // !! IF YOU ADD 2 MEDIA, YOU MAY HAVE ONLY 1 PUBLISHED !!
            if (filepath2 != null)
            {
                byte[] file2 = File.ReadAllBytes(filepath2);
                tweet.AddMedia(file2);
            }

            tweet.Publish();
        }

        private static void Tweet_PublishTweetInReplyToAnotherTweet(string text, long tweetIdtoRespondTo)
        {
            var newTweet = Tweet.CreateTweet(text);
            newTweet.PublishInReplyTo(tweetIdtoRespondTo);

            Console.WriteLine(newTweet.IsTweetPublished);
        }

        private static void Tweet_PublishTweetWithGeo(string text)
        {
            const double longitude = -122.400612831116;
            const double latitude = 37.7821120598956;

            var newTweet = Tweet.CreateTweet(text);
            newTweet.PublishWithGeo(longitude, latitude);

            Console.WriteLine(newTweet.IsTweetPublished);
        }

        private static void Tweet_PublishTweetWithGeo_Coordinates(string text)
        {
            const double longitude = -122.400612831116;
            const double latitude = 37.7821120598956;
            var coordinates = Geo.GenerateCoordinates(longitude, latitude);

            var newTweet = Tweet.CreateTweet(text);
            newTweet.PublishWithGeo(coordinates);

            Console.WriteLine(newTweet.IsTweetPublished);
        }

        private static void Tweet_PublishRetweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            var retweet = tweet.PublishRetweet();

            Console.WriteLine("You retweeted : '{0}'", retweet.Text);
        }

        private static void Tweet_DestroyRetweet(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            var retweet = tweet.PublishRetweet();

            retweet.Destroy();
        }

        private static void Tweet_GetRetweets(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            IEnumerable<ITweet> retweets = tweet.GetRetweets();

            var firstRetweeter = retweets.ElementAt(0).CreatedBy;
            var originalTweet = retweets.ElementAt(0).RetweetedTweet;
            Console.WriteLine("{0} retweeted : '{1}'", firstRetweeter.Name, originalTweet.Text);
        }

        private static void Tweet_GenerateOEmbedTweet()
        {
            var newTweet = Tweet.CreateTweet("to be oembed");
            newTweet.Publish();

            var oembedTweet = newTweet.GenerateOEmbedTweet();

            Console.WriteLine("Oembed tweet url : {0}", oembedTweet.URL);

            if (newTweet.IsTweetPublished)
            {
                newTweet.Destroy();
            }
        }

        private static void Tweet_Destroy()
        {
            var newTweet = Tweet.CreateTweet("to be destroyed!");
            newTweet.Publish();
            bool isTweetPublished = newTweet.IsTweetPublished;

            if (isTweetPublished)
            {
                newTweet.Destroy();
            }

            bool tweetDestroyed = newTweet.IsTweetDestroyed;
            Console.WriteLine("Has the tweet destroyed? {0}", tweetDestroyed);
        }

        private static void Tweet_SetTweetAsFavorite(long tweetId)
        {
            var tweet = Tweet.GetTweet(tweetId);
            tweet.Favourite();
            Console.WriteLine("Is tweet now favourite? -> {0}", tweet.Favourited);
        }

        #endregion

        #region User

        private static void User_GetCurrentUser()
        {
            var user = User.GetLoggedUser();
            Console.WriteLine(user.ScreenName);
        }

        private static void User_GetUserFromId(long userId)
        {
            var user = User.GetUserFromId(userId);
            Console.WriteLine(user.ScreenName);
        }

        private static void User_GetUserFromName(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            Console.WriteLine(user.Id);
        }

        private static void User_GetFriendIds(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var friendIds = user.GetFriendIds();

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friendId in friendIds)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        private static void User_GetFriendIdsUpTo(string userName, int limit)
        {
            var user = User.GetUserFromScreenName(userName);
            var friendIds = user.GetFriendIds(limit);

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friendId in friendIds)
            {
                Console.WriteLine("- {0}", friendId);
            }
        }

        private static void User_GetFriends(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var friends = user.GetFriends();

            Console.WriteLine("{0} has {1} friends, here are some of them :", user.Name, user.FriendsCount);
            foreach (var friend in friends)
            {
                Console.WriteLine("- {0}", friend.Name);
            }
        }

        private static void User_GetFollowerIds(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var followerIds = user.GetFollowerIds();

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var followerId in followerIds)
            {
                Console.WriteLine("- {0}", followerId);
            }
        }

        private static void User_GetFollowerIdsUpTo(string userName, int limit)
        {
            var user = User.GetUserFromScreenName(userName);
            var followerIds = user.GetFollowerIds(limit);

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var followerId in followerIds)
            {
                Console.WriteLine("- {0}", followerId);
            }
        }

        private static void User_GetFollowers(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var followers = user.GetFollowers();

            Console.WriteLine("{0} has {1} followers, here are some of them :", user.Name, user.FollowersCount);
            foreach (var follower in followers)
            {
                Console.WriteLine("- {0}", follower.Name);
            }
        }

        private static void User_GetRelationshipBetween(string sourceUserName, string targetUsername)
        {
            var sourceUser = User.GetUserFromScreenName(sourceUserName);
            var targetUser = User.GetUserFromScreenName(targetUsername);

            var relationship = sourceUser.GetRelationshipWith(targetUser);
            Console.WriteLine("You are{0} following {1}", relationship.Following ? "" : " not", targetUsername);
            Console.WriteLine("You are{0} being followed by {1}", relationship.FollowedBy ? "" : " not", targetUsername);
        }

        private static void User_GetFavorites(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var favorites = user.GetFavorites();

            Console.WriteLine("{0} has {1} favorites, here are some of them :", user.Name, user.FavouritesCount);
            foreach (var favoriteTweet in favorites)
            {
                Console.WriteLine("- {0}", favoriteTweet.Text);
            }
        }

        private static void User_BlockUser(string userName)
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

        private static void User_UnBlockUser(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            user.UnBlockUser();
        }

        private static void User_GetBlockedUsers()
        {
            var loggedUser = User.GetLoggedUser();
            loggedUser.GetBlockedUsers();
            loggedUser.GetBlockedUserIds();
        }

        private static void User_DownloadProfileImage(string userName)
        {
            var user = User.GetUserFromScreenName(userName);
            var stream = user.GetProfileImageStream(ImageSize.bigger);
            var fileStream = new FileStream(String.Format("{0}.jpg", user.Id), FileMode.Create);
            stream.CopyTo(fileStream);

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (assemblyPath != null)
            {
                Process.Start(assemblyPath);
            }
        }

        private static void User_GetMutedUsers()
        {
            var loggedUser = User.GetLoggedUser();
            loggedUser.GetMutedUserIds();
        }

        #endregion

        #region LoggedUser

        private static void LoggedUser_GetIncomingRequests()
        {
            var loggedUser = User.GetLoggedUser();
            var usersRequestingFriendship = loggedUser.GetUsersRequestingFriendship();

            foreach (var user in usersRequestingFriendship)
            {
                Console.WriteLine("{0} wants to follow you!", user.Name);
            }
        }

        private static void LoggedUser_GetOutgoingRequests()
        {
            var loggedUser = User.GetLoggedUser();
            var usersRequestingFriendship = loggedUser.GetUsersYouRequestedToFollow();

            foreach (var user in usersRequestingFriendship)
            {
                Console.WriteLine("You sent a request to follow {0}!", user.Name);
            }
        }

        private static void LoggedUser_FollowUser(string userName)
        {
            var loggedUser = User.GetLoggedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (loggedUser.FollowUser(userToFollow))
            {
                Console.WriteLine("You have successfully sent a request to follow {0}", userToFollow.Name);
            }
        }

        private static void LoggedUser_UnFollowUser(string userName)
        {
            var loggedUser = User.GetLoggedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (loggedUser.UnFollowUser(userToFollow))
            {
                Console.WriteLine("You are not following {0} anymore", userToFollow.Name);
            }
        }

        private static void LoggedUser_UpdateFollowAuthorizationsForUser(string userName)
        {
            var loggedUser = User.GetLoggedUser();
            var userToFollow = User.GetUserFromScreenName(userName);

            if (loggedUser.UpdateRelationshipAuthorizationsWith(userToFollow, false, false))
            {
                Console.WriteLine("Authorizations updated");
            }
        }

        private static void LoggedUser_GetLatestReceivedMessages()
        {
            var loggedUser = User.GetLoggedUser();
            var messages = loggedUser.GetLatestMessagesReceived(20);

            Console.WriteLine("Messages Received : ");
            foreach (var message in messages)
            {
                Console.WriteLine("- '{0}'", message.Text);
            }
        }

        private static void LoggedUser_GetLatestSentMessages()
        {
            var loggedUser = User.GetLoggedUser();
            var messages = loggedUser.GetLatestMessagesSent(20);

            Console.WriteLine("Messages Received : ");
            foreach (var message in messages)
            {
                Console.WriteLine("- '{0}'", message.Text);
            }
        }

        private static void LoggedUser_GetAccountSettings()
        {
            var loggedUser = User.GetLoggedUser();
            var settings = loggedUser.GetAccountSettings();

            // Store information
            loggedUser.AccountSettings = settings;

            Console.WriteLine("{0} uses lang : {1}", settings.ScreenName, settings.Language);
        }

        #endregion

        #region Account & Relationships

        private void Account_GetAndSetAccountSettings()
        {
            var settings = Account.GetCurrentAccountSettings();

            var updatedSettingsRequestParameter = Account.CreateUpdateAccountSettingsRequestParameters(settings);
            updatedSettingsRequestParameter.SleepTimeEnabled = false;

            var updatedSettings = Account.UpdateAccountSettings(updatedSettingsRequestParameter);
            Console.WriteLine(updatedSettings.SleepTimeEnabled);
        }

        private void Account_Mute()
        {
            // Get
            Account.GetMutedUserIds();
            Account.GetMutedUsers();

            // Create
            Account.MuteUser("username");

            // Delete
            Account.UnMuteUser("username");
        }

        private static void Friendship_GetMultipleRelationships()
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

        private static void Friendship_GetUserRequestingFriendship()
        {
            var friendshipRequests = Account.GetUsersRequestingFriendship();

            foreach (var friendshipRequest in friendshipRequests)
            {
                Console.WriteLine("{0} requested to be your friend!", friendshipRequest.Name);
            }
        }

        private static void Friendship_GetUsersYouRequestedToFollow()
        {
            var usersYouWantToFollow = Account.GetUsersYouRequestedToFollow();

            foreach (var user in usersYouWantToFollow)
            {
                Console.WriteLine("You want to be friend with {0}.", user.Name);
            }
        }

        private static void Friendship_GetUsersWhoCantRetweet()
        {
            Account.GetUsersWhoseRetweetsAreMuted();
        }

        private static void Friendship_UpdateRelationship()
        {
            const bool enableRetweets = true;
            const bool enableNotificationsOnDevices = true;

            var success = Account.UpdateRelationshipAuthorizationsWith("tweetinviapi", enableRetweets, enableNotificationsOnDevices);
        }

        private static void Friendship_GetRelationshipDetails()
        {
            var relationshipDetails = Friendship.GetRelationshipDetailsBetween("tweetinviapi", "twitterapi");
        }

        #endregion

        #region Timeline

        private static void Timeline_GetUserTimeline(string username)
        {
            var user = User.GetUserFromScreenName(username);

            var timelineTweets = user.GetUserTimeline();
            foreach (var tweet in timelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        private static void Timeline_GetHomeTimeline()
        {
            var loggedUser = User.GetLoggedUser();

            var homeTimelineTweets = loggedUser.GetHomeTimeline();
            foreach (var tweet in homeTimelineTweets)
            {
                Console.WriteLine(tweet.Text);
            }
        }

        private static void Timeline_GetMentionsTimeline()
        {
            var loggedUser = User.GetLoggedUser();

            var mentionsTimelineTweets = loggedUser.GetMentionsTimeline();
            foreach (var mention in mentionsTimelineTweets)
            {
                Console.WriteLine(mention.Text);
            }
        }

        #endregion

        #region Message

        private static void Message_GetLatests()
        {
            // Messages Received
            var latestMessagesReceived = Message.GetLatestMessagesReceived();
            var latestMessagesReceivedParameter = Message.CreateGetLatestsReceivedRequestParameter();
            latestMessagesReceivedParameter.SinceId = 10029230923;
            var latestMessagesReceivedFromParameter = Message.GetLatestMessagesReceived(latestMessagesReceivedParameter);

            // Messages Sent
            var latestMessagesSent = Message.GetLatestMessagesSent();
            var latestMessagesSentParameter = Message.CreateGetLatestsSentRequestParameter();
            latestMessagesSentParameter.PageNumber = 239823;
            var latestMessagesSentFromParameter = Message.GetLatestMessagesSent(latestMessagesSentParameter);
        }

        private static void Message_GetMessageFromId(long messageId)
        {
            var message = Message.GetExistingMessage(messageId);
            Console.WriteLine("Message from {0} to {1} : {2}", message.Sender, message.Receiver, message.Text);
        }

        private static void Message_DestroyMessageFromId(long messageId)
        {
            var message = Message.GetExistingMessage(messageId);
            if (message.Destroy())
            {
                Console.WriteLine("Message successfully destroyed!");
            }
        }

        private static void Message_PublishMessage(string username)
        {
            var recipient = User.GetUserFromScreenName(username);
            var message = Message.CreateMessage("piloupe", recipient);

            if (message.Publish())
            {
                Console.WriteLine("Message published : {0}", message.IsMessagePublished);
            }
        }

        private static void Message_PublishMessageTo(string username)
        {
            var recipient = User.GetUserFromScreenName(username);
            var message = Message.CreateMessage("piloupe");

            if (message.PublishTo(recipient))
            {
                Console.WriteLine("Message published : {0}", message.IsMessagePublished);
            }
        }

        #endregion

        #region Lists

        private static void TwitterList_GetUserOwnedLists()
        {
            var user = User.GetLoggedUser();
            var ownedLists = TwitterList.GetUserOwnedLists(user);

            ownedLists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        private static void TwitterList_GetUserSubscribedLists()
        {
            var currentUser = User.GetLoggedUser();
            var lists = TwitterList.GetUserSubscribedLists(currentUser);

            lists.ForEach(list => Console.WriteLine("- {0}", list.FullName));
        }

        private static void TwitterList_GetExistingListById(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            Console.WriteLine("You have retrieved the list {0}", list.Name);
        }

        private static void TwitterList_CreateList()
        {
            var list = TwitterList.CreateList("plop", PrivacyMode.Public, "description");
            Console.WriteLine("List '{0}' has been created!", list.FullName);
        }

        private static void TwitterList_UpdateList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var updateParameters = TwitterList.CreateUpdateParameters();
            updateParameters.Name = "piloupe";
            updateParameters.Description = "pilouping description";
            updateParameters.PrivacyMode = PrivacyMode.Private;
            list.Update(updateParameters);

            Console.WriteLine("List new name is : {0}", list.Name);
        }

        private static void TwitterList_DestroyList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var hasBeenDestroyed = list.Destroy();
            Console.WriteLine("Tweet {0} been destroyed.", hasBeenDestroyed ? "has" : "has not");
        }

        private static void TwitterList_GetTweetsFromList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var tweets = list.GetTweets();

            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        private static void TwitterList_GetMembersOfList(long listId)
        {
            var list = TwitterList.GetExistingList(listId);
            var members = list.GetMembers();

            members.ForEach(x => Console.WriteLine(x.Name));
        }

        private static void TwitterList_CheckUserMembership(long userId, long listId)
        {
            var isUserAMember = TwitterList.CheckIfUserIsAListMember(userId, listId);
            Console.WriteLine("{0} is{1}a member of list {2}", userId, isUserAMember ? " " : " NOT ", listId);
        }

        private static void TwitterList_GetSubscribers(long listId)
        {
            var subscribers = TwitterList.GetListSubscribers(listId);

            subscribers.ForEach(user => Console.WriteLine(user));
        }

        private static void TwitterList_SubscribeOrUnsubscribeToList(long listId)
        {
            var hasSuccessfullySubscribed = TwitterList.SubscribeLoggedUserToList(listId);
            var hasUnsubscribed = TwitterList.UnSubscribeLoggedUserToList(listId);
        }

        private static void TwitterList_CheckUserSubscription(long userId, long listId)
        {
            var isUserASubscriber = TwitterList.CheckIfUserIsAListSubscriber(userId, listId);
            Console.WriteLine("{0} is{1}subscribed to the list {2}", userId, isUserASubscriber ? " " : " NOT ", listId);
        }

        #endregion

        #region Geo/Trends

        private static void Geo_GetPlaceFromId(string placeId)
        {
            var geoController = TweetinviContainer.Resolve<IGeoController>();
            var place = geoController.GetPlaceFromId(placeId);
            Console.WriteLine(place.Name);
        }

        private static void Trends_GetTrendsFromWoeId(long woeid)
        {
            var trendsController = TweetinviContainer.Resolve<ITrendsController>();
            var placeTrends = trendsController.GetPlaceTrendsAt(woeid);
            Console.WriteLine(placeTrends.woeIdLocations.First().Name);
        }

        #endregion

        #region Search

        private static void Search_SimpleTweetSearch()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!
            var tweets = Search.SearchTweets("#obama");

            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0}", tweet.Text);
            }
        }

        private static void Search_SearchTweet()
        {
            // IF YOU DO NOT RECEIVE ANY TWEET, CHANGE THE PARAMETERS!

            var searchParameter = Search.CreateTweetSearchParameter("obama");

            searchParameter.SetGeoCode(Geo.GenerateCoordinates(-122.398720, 37.781157), 1, DistanceMeasure.Miles);
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

        private static void Search_SearchWithMetadata()
        {
            Search.SearchTweetsWithMetadata("hello");
        }

        private static void Search_FilteredSearch()
        {
            var searchParameter = Search.CreateTweetSearchParameter("#tweetinvi");
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;

            var tweets = Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        private static void Search_SearchAndGetMoreThan100Results()
        {
            var searchParameter = Search.CreateTweetSearchParameter("us");
            searchParameter.MaximumNumberOfResults = 200;

            var tweets = Search.SearchTweets(searchParameter);
            tweets.ForEach(t => Console.WriteLine(t.Text));
        }

        private static void Search_SearchUsers()
        {
            var users = Search.SearchUsers("linvi", 100);
            users.ForEach(Console.WriteLine);
        }

        #endregion

        #region Saved Searches

        private static void SavedSearch_GetSavedSearches()
        {
            var loggedUser = User.GetLoggedUser();
            var savedSearches = loggedUser.GetSavedSearches();

            Console.WriteLine("Saved Searches");
            foreach (var savedSearch in savedSearches)
            {
                Console.WriteLine("- {0} => {1}", savedSearch.Name, savedSearch.Query);
            }
        }

        private static void SavedSearch_CreateSavedSearch(string query)
        {
            var savedSearch = SavedSearch.CreateSavedSearch(query);
            Console.WriteLine("Saved Search created as : {0}", savedSearch.Name);
        }

        private static void SavedSearch_GetSavedSearch(long searchId)
        {
            var savedSearch = SavedSearch.GetSavedSearch(searchId);
            Console.WriteLine("Saved searched query is : '{0}'", savedSearch.Query);
        }

        private static void SavedSearch_DestroySavedSearch(long searchId)
        {
            var savedSearch = SavedSearch.GetSavedSearch(searchId);

            if (SavedSearch.DestroySavedSearch(savedSearch))
            {
                Console.WriteLine("You destroyed the search successfully!");
            }
        }

        #endregion

        #region Rate Limit / Help

        private static void RateLimits_Track_Examples()
        {
            // Enable Tweetinvi RateLimit Handler
            RateLimit.RateLimitTrackerOption = RateLimitTrackerOptions.TrackAndAwait;

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

        private static void RateLimits_ManualAwait()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) =>
           {
               var queryRateLimit = RateLimit.GetQueryRateLimit(args.QueryURL);
               RateLimit.AwaitForQueryRateLimit(queryRateLimit);
           };
        }

        private static void GetCurrentCredentialsRateLimits()
        {
            var tokenRateLimits = RateLimit.GetCurrentCredentialsRateLimits();

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToLongTimeString());
        }

        private static void GetCredentialsRateLimits()
        {
            var credentials = TwitterCredentials.CreateCredentials("ACCESS_TOKEN", "ACCESS_TOKEN_SECRET", "CONSUMER_KEY", "CONSUMER_SECRET");
            var tokenRateLimits = RateLimit.GetCredentialsRateLimits(credentials);

            Console.WriteLine("Remaning Requests for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            Console.WriteLine("Total Requests Allowed for GetRate : {0}", tokenRateLimits.ApplicationRateLimitStatusLimit.Limit);
            Console.WriteLine("GetRate limits will reset at : {0} local time", tokenRateLimits.ApplicationRateLimitStatusLimit.ResetDateTime.ToLongTimeString());
        }

        private static void GetTwitterPrivacyPolicy()
        {
            Console.WriteLine(Help.GetTwitterPrivacyPolicy());
        }

        #endregion

        #region Json

        private static void Json_GetJsonForAccountRequestExample()
        {
            string jsonResponse = AccountJson.GetLoggedUserSettingsJson();
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForMessageRequestExample()
        {
            IUser user = User.GetUserFromScreenName("tweetinviapi");
            string jsonResponse = MessageJson.PublishMessage("salut", user.UserDTO);

            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForGeoRequestExample()
        {
            var jsonResponse = GeoJson.GetPlaceFromId("df51dec6f4ee2b2c");
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForHelpRequestExample()
        {
            var jsonResponse = HelpJson.GetTokenRateLimits();
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForSavedSearchRequestExample()
        {
            var jsonResponse = SavedSearchJson.GetSavedSearches();
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForTimelineRequestExample()
        {
            var jsonResponse = TimelineJson.GetHomeTimeline(2);
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForTrendsRequestExample()
        {
            var jsonResponse = TrendsJson.GetTrendsAt(1);
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForTweetRequestExample()
        {
            var tweet = Tweet.CreateTweet("Hello there!");
            var jsonResponse = TweetJson.PublishTweet(tweet);
            Console.WriteLine(jsonResponse);
        }

        private static void Json_GetJsonForUserRequestExample()
        {
            var loggedUser = User.GetLoggedUser();
            var jsonResponse = UserJson.GetFriendIds(loggedUser);
            Console.WriteLine(jsonResponse.ElementAt(0));
        }

        private static void Json_GetJsonCursorRequestExample()
        {
            // This query is a cursor query
            var jsonResponses = FriendshipJson.GetUserIdsRequestingFriendship();

            foreach (var jsonResponse in jsonResponses)
            {
                Console.WriteLine(jsonResponse);
            }
        }

        #endregion

        #region Stream

        private static void SimpleStream_Events()
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

        private static void Stream_SampleStreamExample()
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

        private static void Stream_FilteredStreamExample()
        {
            var stream = Stream.CreateFilteredStream();
            var location = Geo.GenerateLocation(-124.75, 36.8, -126.89, 32.75);

            stream.AddLocation(location);
            stream.AddTrack("tweetinvi");
            stream.AddTrack("linvi");

            stream.MatchingTweetAndLocationReceived += (sender, args) =>
            {
                var tweet = args.Tweet;
                Console.WriteLine("{0} was detected between the following tracked locations:", tweet.Id);

                IEnumerable<ILocation> matchingLocations = args.MatchedLocations;
                foreach (var matchingLocation in matchingLocations)
                {
                    Console.Write("({0}, {1}) ;", matchingLocation.Coordinate1.Latitude, matchingLocation.Coordinate1.Longitude);
                    Console.WriteLine("({0}, {1})", matchingLocation.Coordinate2.Latitude, matchingLocation.Coordinate2.Longitude);
                }
            };

            stream.StartStreamMatchingAllConditions();
        }

        private static void Stream_UserStreamExample()
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
            userStream.LoggedUserProfileUpdated += (sender, args) =>
            {
                var newLoggedUser = args.LoggedUser;
                Console.WriteLine("Logged user '{0}' has been updated!", newLoggedUser.Name);
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

        private static void EventsRelatedWithTweetCreation(IUserStream userStream)
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

        private static void EventsRelatedWithMessages(IUserStream userStream)
        {
            userStream.MessageSent += (sender, args) => { Console.WriteLine("message '{0}' sent", args.Message.Text); };
            userStream.MessageReceived += (sender, args) => { Console.WriteLine("message '{0}' received", args.Message.Text); };
        }

        private static void EventsRelatedWithTweetAndFavourite(IUserStream userStream)
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
                var loggedUser = args.FavouritingUser;
                Console.WriteLine("Logged User '{0}' favourited tweet '{1}'", loggedUser.Name, tweet.Id);
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

        private static void EventsRelatedWithLists(IUserStream userStream)
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
            userStream.LoggedUserAddedMemberToList += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You added '{0}' to the list : '{1}'", newUser.Name, list.Name);
            };

            userStream.LoggedUserAddedToListBy += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You haved been added to the list '{0}' by '{1}'", list.Name, newUser.Name);
            };

            // User Removed
            userStream.LoggedUserRemovedMemberFromList += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You removed '{0}' from the list : '{1}'", newUser.Name, list.Name);
            };

            userStream.LoggedUserRemovedFromListBy += (sender, args) =>
            {
                var newUser = args.User;
                var list = args.List;
                Console.WriteLine("You haved been removed from the list '{0}' by '{1}'", list.Name, newUser.Name);
            };

            // User Subscribed
            userStream.LoggedUserSubscribedToListCreatedBy += (sender, args) =>
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
            userStream.LoggedUserUnsubscribedToListCreatedBy += (sender, args) =>
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

        private static void EventsRelatedWithBlock(IUserStream userStream)
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

        #region Exception

        private static void Exceptions_GetExceptionsInfo()
        {
            TwitterCredentials.Credentials = null;

            var user = User.GetLoggedUser();
            if (user == null)
            {
                var lastException = ExceptionHandler.GetLastException();
                Console.WriteLine(lastException.TwitterDescription);
            }
        }

        #endregion

        #region Manual Query

        private static void ManualQuery_Example()
        {
            const string getHomeTimelineQuery = "https://api.twitter.com/1.1/statuses/home_timeline.json";

            // Execute Query can either return a json or a DTO interface
            var tweetsDTO = TwitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(getHomeTimelineQuery);
            tweetsDTO.ForEach(tweetDTO => Console.WriteLine(tweetDTO.Text));
        }

        #endregion

        #region Configuration (Timeout/Proxy)

        private static void ConfigureTweetinvi()
        {
            // TweetinviConfig.CURRENT_PROXY_URL = "http://228.23.13.21:4287";

            // Configure a proxy with Proxy with username and password
            // TweetinviConfig.CURRENT_PROXY_URL = "http://user:pass@228.23.13.21:4287";

            // TweetinviConfig.CURRENT_WEB_REQUEST_TIMEOUT = 5000;
            // TweetinviConfig.CURRENT_SHOW_DEBUG = false;
        }

        private static void GlobalEvents()
        {
            //TweetinviEvents.QueryBeforeExecute += (sender, args) =>
            //{
            //    Console.WriteLine("The query {0} is about to be executed.", args.Query);
            //};

            //TweetinviEvents.QueryAfterExecute += (sender, args) =>
            //{
            //    Console.WriteLine("The query {0} has just been executed.", args.Query);
            //};
        }

        #endregion
    }
}