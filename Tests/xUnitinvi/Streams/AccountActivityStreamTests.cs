using System;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.Streams
{
    public class AccountActivityStreamTests
    {
        private const int ACCOUNT_ACTIVITY_USER_ID = 42;

        private static IAccountActivityStream CreateAccountActivityStream()
        {
			var client = new TwitterClient("", "");
            var stream = client.CreateTwitterExecutionContext().Container.Resolve<IAccountActivityStream>();
            stream.AccountUserId = ACCOUNT_ACTIVITY_USER_ID;
            return stream;
        }

        [Fact]
        public void TweetEventRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
	            ""for_user_id"": ""100"",
	            ""tweet_create_events"": [
	              " + JsonTests.TWEET_TEST_JSON + @"
	            ]
            }";

            var eventsReceived = new List<TweetCreatedEvent>();
            activityStream.TweetCreated += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert

            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].Tweet.CreatedBy.Id, 42);
        }

        [Fact]
        public void TweetDeletedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
                ""for_user_id"": ""3198576760"",
                ""tweet_delete_events"": [
            {
                    ""status"": {
                        ""id"": ""601430178305220608"",
                        ""user_id"": ""3198576760""
                    },
                    ""timestamp_ms"": ""1432228155593""
                }
               ]
            }";

            var eventsReceived = new List<TweetDeletedEvent>();
            activityStream.TweetDeleted += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].UserId, 3198576760);
            Assert.Equal(eventsReceived[0].TweetId, 601430178305220608);
            var time = TimeSpan.FromMilliseconds(1432228155593);
            Assert.Equal(eventsReceived[0].EventDate, new DateTime(1970, 1, 1) + time);
        }

        [Fact]
        public void FavouriteTweetRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetFavouritedJson = @"{
	            ""for_user_id"": ""100"",
	            ""favorite_events"": [{
                  ""favorited_status"" : " + JsonTests.TWEET_TEST_JSON + @",
                  ""user"": " + JsonTests.USER_TEST_JSON(4242) + @"
	            }]
            }";

            var eventsReceived = new List<TweetFavouritedEvent>();
            activityStream.TweetFavourited += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetFavouritedJson));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].Tweet.CreatedBy.Id, 42);
            Assert.Equal(eventsReceived[0].FavouritedBy.Id, 4242);
        }

        [Fact]
        public void UserFollowedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""follow_events"": [
			        {
			            ""type"": ""follow"",
			            ""created_timestamp"": ""1586216143967"",
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 12,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            },
			            ""source"": {
			                ""id"": ""1245855954975051776"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 1,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserFollowedEvent>();
            activityStream.UserFollowed += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].FollowedBy.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].FollowedUser.Id, 1693649419);
        }

        [Fact]
        public void UserUnfollowedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""follow_events"": [
			        {
			            ""type"": ""unfollow"",
			            ""created_timestamp"": ""1586216162049"",
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 11,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            },
			            ""source"": {
			                ""id"": ""1245855954975051776"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserUnfollowedEvent>();
            activityStream.UserUnfollowed += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].UnfollowedBy.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].UnfollowedUser.Id, 1693649419);
        }

        [Fact]
        public void UserBlockedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""42"",
			    ""block_events"": [
			        {
			            ""type"": ""block"",
			            ""created_timestamp"": ""1586215587741"",
			            ""source"": {
			                ""id"": ""42"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            },
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 11,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserBlockedEvent>();
            activityStream.UserBlocked += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].BlockedBy.Id, 42);
            Assert.Equal(eventsReceived[0].BlockedUser.Id, 1693649419);
        }

        [Fact]
        public void UserUnBlockedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""block_events"": [
			        {
			            ""type"": ""unblock"",
			            ""created_timestamp"": ""1586215606091"",
			            ""source"": {
			                ""id"": ""1245855954975051776"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            },
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 11,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserUnblockedEvent>();
            activityStream.UserUnblocked += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].UnblockedBy.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].UnblockedUser.Id, 1693649419);
        }

        [Fact]
        public void UserMutedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""mute_events"": [
			        {
			            ""type"": ""mute"",
			            ""created_timestamp"": ""1586216584460"",
			            ""source"": {
			                ""id"": ""1245855954975051776"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            },
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 11,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserMutedEvent>();
            activityStream.UserMuted += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].MutedBy.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].MutedUser.Id, 1693649419);
        }

         [Fact]
        public void UserUnmutedRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""mute_events"": [
			        {
			            ""type"": ""unmute"",
			            ""created_timestamp"": ""1586216689931"",
			            ""source"": {
			                ""id"": ""1245855954975051776"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 0,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""test_twit_42"",
			                ""is_translator"": false,
			                ""followers_count"": 0,
			                ""name"": ""test42"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Thu Apr 02 23:29:52 +0000 2020"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": true,
			                ""statuses_count"": 0,
			                ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""old_loc"",
			                ""follow_request_sent"": false,
			                ""description"": ""old_desc"",
			                ""profile_background_color"": -1
			            },
			            ""target"": {
			                ""id"": ""1693649419"",
			                ""default_profile_image"": false,
			                ""profile_background_image_url"": """",
			                ""friends_count"": 0,
			                ""favourites_count"": 17,
			                ""profile_link_color"": -1,
			                ""profile_background_image_url_https"": """",
			                ""utc_offset"": 0,
			                ""screen_name"": ""tweetinvitest"",
			                ""is_translator"": false,
			                ""followers_count"": 11,
			                ""name"": ""tweetinvitest"",
			                ""lang"": """",
			                ""profile_use_background_image"": false,
			                ""created_at"": ""Fri Aug 23 11:45:45 +0000 2013"",
			                ""profile_text_color"": -1,
			                ""notifications"": false,
			                ""protected"": false,
			                ""statuses_count"": 249,
			                ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			                ""contributors_enabled"": false,
			                ""default_profile"": true,
			                ""profile_sidebar_border_color"": -1,
			                ""time_zone"": """",
			                ""geo_enabled"": false,
			                ""verified"": false,
			                ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""following"": false,
			                ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			                ""profile_background_tile"": false,
			                ""listed_count"": 0,
			                ""profile_sidebar_fill_color"": -1,
			                ""location"": ""London"",
			                ""follow_request_sent"": false,
			                ""description"": ""test description"",
			                ""profile_background_color"": -1
			            }
			        }
			    ]
			}";

            var eventsReceived = new List<UserUnmutedEvent>();
            activityStream.UserUnmuted += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].UnmutedBy.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].UnmutedUser.Id, 1693649419);
        }

        [Fact]
        public void DirectMessageReceived()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""42"",
			    ""direct_message_events"": [
			        {
			            ""type"": ""message_create"",
			            ""id"": ""1247299084244979717"",
			            ""created_timestamp"": ""1586214250037"",
			            ""message_create"": {
			                ""target"": {
			                    ""recipient_id"": ""42""
			                },
			                ""sender_id"": ""1693649419"",
			                ""message_data"": {
			                    ""text"": ""Hello World!"",
			                    ""entities"": {
			                        ""hashtags"": [],
			                        ""symbols"": [],
			                        ""user_mentions"": [],
			                        ""urls"": []
			                    }
			                }
			            }
			        }
			    ],
			    ""users"": {
			        ""1693649419"": {
			            ""id"": ""1693649419"",
			            ""created_timestamp"": ""1377258345080"",
			            ""name"": ""tweetinvitest"",
			            ""screen_name"": ""tweetinvitest"",
			            ""location"": ""London"",
			            ""description"": ""test description"",
			            ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			            ""protected"": false,
			            ""verified"": false,
			            ""followers_count"": 11,
			            ""friends_count"": 0,
			            ""statuses_count"": 249,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg""
			        },
			        ""42"": {
			            ""id"": ""42"",
			            ""created_timestamp"": ""1585870192199"",
			            ""name"": ""test42"",
			            ""screen_name"": ""test_twit_42"",
			            ""location"": ""old_loc"",
			            ""description"": ""old_desc"",
			            ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			            ""protected"": true,
			            ""verified"": false,
			            ""followers_count"": 0,
			            ""friends_count"": 0,
			            ""statuses_count"": 0,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png""
			        }
			    }
			}";

            var eventsReceived = new List<MessageReceivedEvent>();
            activityStream.MessageReceived += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].Message.Text, "Hello World!");
            Assert.Equal(eventsReceived[0].Message.SenderId, 1693649419);
        }

        [Fact]
        public void DirectMessageSent()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""42"",
			    ""direct_message_events"": [
			        {
			            ""type"": ""message_create"",
			            ""id"": ""1247295861006241798"",
			            ""created_timestamp"": ""1586213481557"",
			            ""message_create"": {
			                ""target"": {
			                    ""recipient_id"": ""1693649419""
			                },
			                ""sender_id"": ""42"",
			                ""source_app_id"": ""3033300"",
			                ""message_data"": {
			                    ""text"": ""hello there"",
			                    ""entities"": {
			                        ""hashtags"": [],
			                        ""symbols"": [],
			                        ""user_mentions"": [],
			                        ""urls"": []
			                    }
			                }
			            }
			        }
			    ],
			    ""apps"": {
			        ""3033300"": {
			            ""id"": ""3033300"",
			            ""name"": ""The tweetinvi app"",
			            ""url"": ""https:\/\/mobile.twitter.com""
			        }
			    },
			    ""users"": {
			        ""42"": {
			            ""id"": ""42"",
			            ""created_timestamp"": ""1585870192199"",
			            ""name"": ""test42"",
			            ""screen_name"": ""test_twit_42"",
			            ""location"": ""old_loc"",
			            ""description"": ""old_desc"",
			            ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			            ""protected"": true,
			            ""verified"": false,
			            ""followers_count"": 0,
			            ""friends_count"": 0,
			            ""statuses_count"": 0,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png""
			        },
			        ""1693649419"": {
			            ""id"": ""1693649419"",
			            ""created_timestamp"": ""1377258345080"",
			            ""name"": ""tweetinvitest"",
			            ""screen_name"": ""tweetinvitest"",
			            ""location"": ""London"",
			            ""description"": ""test description"",
			            ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			            ""protected"": false,
			            ""verified"": false,
			            ""followers_count"": 11,
			            ""friends_count"": 0,
			            ""statuses_count"": 249,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg""
			        }
			    }
			}";

            var eventsReceived = new List<MessageSentEvent>();
            activityStream.MessageSent += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].Message.Text, "hello there");
            Assert.Equal(eventsReceived[0].Message.SenderId, 42);
            Assert.Equal(eventsReceived[0].Message.App.Name, "The tweetinvi app");
        }

        [Fact]
        public void UserIsTypingDirectMessage()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
	            ""for_user_id"": ""4337869213"",
	            ""direct_message_indicate_typing_events"": [{
		            ""created_timestamp"": ""1518127183443"",
		            ""sender_id"": ""3284025577"",
		            ""target"": {
			            ""recipient_id"": ""3001969357""
		            }
	            }],
	            ""users"": {
		            ""3001969357"": {
			            ""id"": ""3001969357"",
			            ""created_timestamp"": ""1422556069340"",
			            ""name"": ""Jordan Brinks"",
			            ""screen_name"": ""furiouscamper"",
			            ""location"": ""Boulder, CO"",
			            ""description"": ""Alter Ego - Twitter PE opinions-are-my-own"",
			            ""url"": ""https://t.co/SnxaA15ZuY"",
			            ""protected"": false,
			            ""verified"": false,
			            ""followers_count"": 23,
			            ""friends_count"": 47,
			            ""statuses_count"": 509,
			            ""profile_image_url"": ""http://pbs.twimg.com/profile_images/851526626785480705/cW4WTi7C_normal.jpg"",
			            ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/851526626785480705/cW4WTi7C_normal.jpg""
		            },
		            ""3284025577"": {
			            ""id"": ""3284025577"",
			            ""created_timestamp"": ""1437281176085"",
			            ""name"": ""Bogus Bogart"",
			            ""screen_name"": ""emilyannsheehan"",
			            ""protected"": true,
			            ""verified"": false,
			            ""followers_count"": 1,
			            ""friends_count"": 4,
			            ""statuses_count"": 35,
			            ""profile_image_url"": ""http://pbs.twimg.com/profile_images/763383202857779200/ndvZ96mE_normal.jpg"",
			            ""profile_image_url_https"": ""https://pbs.twimg.com/profile_images/763383202857779200/ndvZ96mE_normal.jpg""
		            }
	            }
            }";

            var eventsReceived = new List<UserIsTypingMessageEvent>();
            activityStream.UserIsTypingMessage += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].TypingUser.Id, 3284025577);
            Assert.Equal(eventsReceived[0].TypingTo.Id, 3001969357);
        }

        [Fact]
        public void UserReadMessage()
        {
            var activityStream = CreateAccountActivityStream();

            var json = @"{
			    ""for_user_id"": ""1245855954975051776"",
			    ""direct_message_mark_read_events"": [
			        {
			            ""created_timestamp"": ""1586216882004"",
			            ""sender_id"": ""1693649419"",
			            ""target"": {
			                ""recipient_id"": ""1245855954975051776""
			            },
			            ""last_read_event_id"": ""1247310121283321862""
			        }
			    ],
			    ""users"": {
			        ""1693649419"": {
			            ""id"": ""1693649419"",
			            ""created_timestamp"": ""1377258345080"",
			            ""name"": ""tweetinvitest"",
			            ""screen_name"": ""tweetinvitest"",
			            ""location"": ""London"",
			            ""description"": ""test description"",
			            ""url"": ""https:\/\/t.co\/kcFUegjBmg"",
			            ""protected"": false,
			            ""verified"": false,
			            ""followers_count"": 11,
			            ""friends_count"": 0,
			            ""statuses_count"": 249,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/961033553294356480\/j92hWbGY_normal.jpg""
			        },
			        ""1245855954975051776"": {
			            ""id"": ""1245855954975051776"",
			            ""created_timestamp"": ""1585870192199"",
			            ""name"": ""test42"",
			            ""screen_name"": ""test_twit_42"",
			            ""location"": ""old_loc"",
			            ""description"": ""old_desc"",
			            ""url"": ""https:\/\/t.co\/cHZdP4MpC4"",
			            ""protected"": true,
			            ""verified"": false,
			            ""followers_count"": 0,
			            ""friends_count"": 0,
			            ""statuses_count"": 0,
			            ""profile_image_url"": ""http:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png"",
			            ""profile_image_url_https"": ""https:\/\/pbs.twimg.com\/profile_images\/1247261816935383040\/BrvKSXnk_normal.png""
			        }
			    }
			}";

            var eventsReceived = new List<UserReadMessageConversationEvent>();
            activityStream.UserReadMessage += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(json));

            // Assert
            Assert.Equal(eventsReceived.Count, 1);
            Assert.Equal(eventsReceived[0].UserWhoWroteTheMessage.Id, 1245855954975051776);
            Assert.Equal(eventsReceived[0].UserWhoReadTheMessageConversation.Id, 1693649419);
            Assert.Equal(eventsReceived[0].LastReadEventId, "1247310121283321862");
        }
    }
}
