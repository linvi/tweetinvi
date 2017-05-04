using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Wrapper;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweet = Tweetinvi.Logic.Tweet;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class ExtendedTweetTests
    {
        [TestMethod]
        public void TestWithSimpleContent()
        {
            var content = "Tweetinvi I love it!";
            var parts = StringExtension.TweetParts(content);

            Assert.AreEqual(parts.Content, content);
            Assert.AreEqual(parts.Prefix, "");
            Assert.AreEqual(parts.Mentions.Length, 0);
        }

        [TestMethod]
        public void TestWithPrefixAndContent()
        {
            var text = "@tweetinviapi Tweetinvi I love it!";
            var parts = StringExtension.TweetParts(text);

            Assert.AreEqual(parts.Content, "Tweetinvi I love it!");
            Assert.AreEqual(parts.Prefix, "@tweetinviapi ");
            Assert.AreEqual(parts.Mentions.Length, 1);
        }

        [TestMethod]
        public void TestWithPrefixAndContentAndSuffix()
        {
            var text = "@sam @aileen " +
                       "Check out this photo of @YellowstoneNPS! " +
                       "It makes me want to go camping there this summer. " +
                       "Have you visited before?? nps.gov/yell/index.htm";

            var parts = StringExtension.TweetParts(text);

            Assert.AreEqual(parts.Prefix, "@sam @aileen ");
            Assert.AreEqual(parts.Prefix.Length, 13);
            
            Assert.AreEqual(parts.Content.TweetLength(), 140);
            Assert.AreEqual(parts.Mentions.Length, 2);
        }

        [TestMethod]
        public void TwitterExample()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            var tweet = InitTweet(_twitterExample);

            Assert.AreEqual(tweet.Prefix, "@sam @aileen ");
            Assert.AreEqual(tweet.Prefix.Length, 13);
            Assert.AreEqual(tweet.Text, "Check out this photo of @YellowstoneNPS! It makes me want to go camping there this summer. Have you visited before?? nps.gov/yell/index.htm ");
            Assert.AreEqual(tweet.Text.Length, 140);
            Assert.AreEqual(tweet.Suffix, "pic.twitter.com/e8bDiL6LI4");
            Assert.AreEqual(tweet.Suffix.Length, 26);
        }

        [TestMethod]
        public void SimpleFullText()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            var tweet = InitTweet(_simpleFullTextExtendedTweet);

            Assert.AreEqual(tweet.Prefix, "");
            Assert.AreEqual(tweet.Text, "Peek-a-boo! https://t.co/R3P6waHxRa");
            Assert.AreEqual(tweet.Suffix, "");
        }

        [TestMethod]
        public void FullTextWithMentionAndSuffixUrl()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            var tweet = InitTweet(_fullTextTweetWith1Mention);

            Assert.AreEqual(tweet.Prefix, "@jeremycloud ");
            Assert.AreEqual(tweet.Text, "Who would win in a battle between a Barred Owl and a Cooper`s Hawk? ");
            Assert.AreEqual(tweet.Suffix, "https://t.co/FamikDro2h");
        }

        [TestMethod]
        public void ExtendedTweet()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            var tweet = InitTweet(_extendedTweet);

            Assert.AreNotEqual(tweet.TweetDTO.ExtendedTweet, null);
            Assert.AreEqual(tweet.FullText, "@jeremycloud It`s neat to have owls and raccoons around until you realize that raccoons will eat the eggs from the owl`s nest https://t.co/Q0pkaU4ORH");
            Assert.AreEqual(tweet.Entities.UserMentions.Count, 1);
        }

        [TestMethod]
        public void SimpleTweetThreePhotos()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            ITweet tweet = InitTweet(SIMPLE_TWEET_THREE_PHOTOS);

            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);
            Assert.AreEqual("hmm https://t.co/vzh4H9Lj1u", tweet.FullText);
            Assert.AreEqual(3, tweet.Media.Count);
        }

        [TestMethod]
        public void SimpleTweetOnePhoto()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            ITweet tweet = InitTweet(SIMPLE_TWEET_ONE_PHOTO);

            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);
            Assert.AreEqual("just one photo https://t.co/blFtRxZnUc", tweet.FullText);
            Assert.AreEqual(1, tweet.Media.Count);
        }

        [TestMethod]
        public void SimpleTweetTwoUrls()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            ITweet tweet = InitTweet(SIMPLE_TWEET_TWO_URLS);

            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);
            Assert.AreEqual("test with some URLs https://t.co/dWeOSNqwo5 https://t.co/KBtBq1lAhx", tweet.FullText);
            Assert.AreEqual(2, tweet.Urls.Count);
        }

        [TestMethod]
        public void SimpleTweetMixedEntities()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            ITweet tweet = InitTweet(SIMPLE_TWEET_MIXED_ENTITIES);

            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);
            Assert.AreEqual("mixed test @VISAV2 @nickvisav https://t.co/FpLiZ4h8QK https://t.co/KNv4CvJhfl yup https://t.co/xckf5pX8gR", tweet.FullText);
            Assert.AreEqual(2, tweet.Urls.Count);
            Assert.AreEqual(2, tweet.UserMentions.Count);
            Assert.AreEqual(3, tweet.Media.Count);
        }

        [TestMethod]
        public void ExtendedTweetThreePhotosFetchedInExtendedMode()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

            ITweet tweet = InitTweet(EXTENDED_TWEET_THREE_PHOTOS_FETCHED_IN_EXTENDED_MODE);

            // This may be an extended Tweet, but since it was fetched over the REST API it doesn't have the ExtendedTweet property
            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);

            Assert.AreEqual("some long message with images at the end some long message with images at the end some long message with images at the end 12oi3j12o3ij3 end https://t.co/tS5h4pyfG8", tweet.FullText);
            Assert.AreEqual(3, tweet.Media.Count);
        }

        [TestMethod]
        public void ExtendedTweetThreePhotosFetchedInCompatMode()
        {
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Compat;

            ITweet tweet = InitTweet(EXTENDED_TWEET_THREE_PHOTOS_FETCHED_IN_COMPAT_MODE);

            Assert.IsNull(tweet.TweetDTO.ExtendedTweet);
            Assert.AreEqual("some long message with images at the end some long message with images at the end some long message with images at… https://t.co/ImGDtYUGc1", tweet.Text);

            // Tweet has media, but since it's pushed off of the end of the compatible text, we actually just get a URL to the full status
            Assert.IsNull(tweet.Media);
            Assert.AreEqual(1, tweet.Urls.Count);
            Assert.AreEqual(tweet.IdStr, tweet.Urls[0].ExpandedURL.Split('/').Last());
        }

        private static ITweet InitTweet(string text)
        {
            var userFactory = A.Fake<IUserFactory>();
            userFactory.CallsTo(x => x.GenerateUserFromDTO(It.IsAny<IUserDTO>())).Returns(null);

            var jsonConverterLib = new JsonConvertWrapper();
            var jsonConverter = new JsonObjectConverter(jsonConverterLib);

            var tweetDTO = jsonConverter.DeserializeObject<ITweetDTO>(text);
            ITweetinviSettingsAccessor tweetinviSettingsAccessor =
                TweetinviContainer.Resolve<ITweetinviSettingsAccessor>();
            return new Tweet(tweetDTO, null, null, userFactory, null, tweetinviSettingsAccessor);
        }

        #region tweet examples

        private static string _twitterExample = @"{
            'id': 42,
            'full_text': '@sam @aileen Check out this photo of @YellowstoneNPS! It makes me want to go camping there this summer. Have you visited before?? nps.gov/yell/index.htm pic.twitter.com/e8bDiL6LI4',
            'display_text_range': [13,152]
        }";

        private static string _extendedTweet = @"{
          'created_at': 'Mon Mar 28 14:39:13 +0000 2016',
          'id': 714461850188926976,
          'id_str': '714461850188926976',
          'text': '@jeremycloud It`s neat to have owls and raccoons around until you realize that raccoons will eat the eggs from the … https://t.co/OY7qmdJQnO',
          'entities': {
            'hashtags': [],
            'symbols': [],
            'user_mentions': [
              {
                'screen_name': 'jeremycloud',
                'name': '/dev/cloud/jeremy',
                'id': 15062340,
                'id_str': '15062340',
                'indices': [
                  0,
                  12
                ]
              }
            ],
            'urls': [
              {
                'url': 'https://t.co/OY7qmdJQnO',
                'expanded_url': 'https://twitter.com/i/web/status/714461850188926976',
                'display_url': 'twitter.com/i/web/status/7…',
                'indices': [
                  117,
                  140
                ]
              }
            ]
          },
          'truncated': true,
          'extended_tweet': {
            'full_text': '@jeremycloud It`s neat to have owls and raccoons around until you realize that raccoons will eat the eggs from the owl`s nest https://t.co/Q0pkaU4ORH',
            'display_text_range': [
              13,
              125
            ],
            'entities': {
              'hashtags': [],
              'symbols': [],
              'user_mentions': [
                {
                  'screen_name': 'jeremycloud',
                  'name': '/dev/cloud/jeremy',
                  'id': 15062340,
                  'id_str': '15062340',
                  'indices': [
                    0,
                    12
                  ]
                }
              ],
              'urls': [
                {
                  'url': 'https://t.co/Q0pkaU4ORH',
                  'expanded_url': 'https://twitter.com/jeremycloud/status/704059336788606976',
                  'display_url': 'twitter.com/jeremycloud/st…',
                  'indices': [
                    126,
                    149
                  ]
                }
              ]
            }
          },
          'source': '<a href=\'http://twitter.com\' rel=\'nofollow\'>Twitter Web Client</a>',
          'in_reply_to_status_id': 706860403981099008,
          'in_reply_to_status_id_str': '706860403981099008',
          'in_reply_to_user_id': 15062340,
          'in_reply_to_user_id_str': '15062340',
          'in_reply_to_screen_name': 'jeremycloud',
          'user': {
            'id': 4449621923,
            'id_str': '4449621923',
            'name': 'Mr Bones',
            'screen_name': 'MrBonesDroid',
            'location': '',
            'profile_location': null,
            'description': '',
            'url': null,
            'entities': {
            'description': {
	        'urls': []
             }
            },
            'protected': true,
            'followers_count': 5,
            'friends_count': 7,
            'listed_count': 0,
            'created_at': 'Fri Dec 11 15:18:02 +0000 2015',
            'favourites_count': 7,
            'utc_offset': -25200,
            'time_zone': 'Pacific Time (US & Canada)',
            'geo_enabled': false,
            'verified': false,
            'statuses_count': 35,
            'lang': 'en-gb',
            'contributors_enabled': false,
            'is_translator': false,
            'is_translation_enabled': false,
            'profile_background_color': 'F5F8FA',
            'profile_background_image_url': null,
            'profile_background_image_url_https': null,
            'profile_background_tile': false,
            'profile_image_url': 'http://pbs.twimg.com/profile_images/677097663288860672/zZxWCPSI_normal.jpg',
            'profile_image_url_https': 'https://pbs.twimg.com/profile_images/677097663288860672/zZxWCPSI_normal.jpg',
            'profile_link_color': '2B7BB9',
            'profile_sidebar_border_color': 'C0DEED',
            'profile_sidebar_fill_color': 'DDEEF6',
            'profile_text_color': '333333',
            'profile_use_background_image': true,
            'has_extended_profile': false,
            'default_profile': true,
            'default_profile_image': false,
            'following': true,
            'follow_request_sent': false,
            'notifications': false
          },
          'geo': null,
          'coordinates': null,
          'place': null,
          'contributors': null,
          'quoted_status_id': 704059336788606976,
          'quoted_status_id_str': '704059336788606976',
          'quoted_status': {
            'created_at': 'Sun Feb 28 21:43:21 +0000 2016',
            'id': 704059336788606976,
            'id_str': '704059336788606976',
            'text': 'My favorite photographic subject, up closer than ever before. https://t.co/K958bKh9Sd',
            'entities': {
              'hashtags': [],
              'symbols': [],
              'user_mentions': [],
              'urls': [],
              'media': [
                {
                  'id': 704059330149031936,
                  'id_str': '704059330149031936',
                  'indices': [
                    62,
                    85
                  ],
                  'media_url': 'http://pbs.twimg.com/media/CcVSOwJVIAAKwE6.jpg',
                  'media_url_https': 'https://pbs.twimg.com/media/CcVSOwJVIAAKwE6.jpg',
                  'url': 'https://t.co/K958bKh9Sd',
                  'display_url': 'pic.twitter.com/K958bKh9Sd',
                  'expanded_url': 'http://twitter.com/jeremycloud/status/704059336788606976/photo/1',
                  'type': 'photo',
                  'sizes': {
                    'medium': {
                      'w': 600,
                      'h': 600,
                      'resize': 'fit'
                    },
                    'thumb': {
                      'w': 150,
                      'h': 150,
                      'resize': 'crop'
                    },
                    'large': {
                      'w': 871,
                      'h': 871,
                      'resize': 'fit'
                    },
                    'small': {
                      'w': 340,
                      'h': 340,
                      'resize': 'fit'
                    }
                  }
                }
              ]
            },
            'extended_entities': {
              'media': [
                {
                  'id': 704059330149031936,
                  'id_str': '704059330149031936',
                  'indices': [
                    62,
                    85
                  ],
                  'media_url': 'http://pbs.twimg.com/media/CcVSOwJVIAAKwE6.jpg',
                  'media_url_https': 'https://pbs.twimg.com/media/CcVSOwJVIAAKwE6.jpg',
                  'url': 'https://t.co/K958bKh9Sd',
                  'display_url': 'pic.twitter.com/K958bKh9Sd',
                  'expanded_url': 'http://twitter.com/jeremycloud/status/704059336788606976/photo/1',
                  'type': 'photo',
                  'sizes': {
                    'medium': {
                      'w': 600,
                      'h': 600,
                      'resize': 'fit'
                    },
                    'thumb': {
                      'w': 150,
                      'h': 150,
                      'resize': 'crop'
                    },
                    'large': {
                      'w': 871,
                      'h': 871,
                      'resize': 'fit'
                    },
                    'small': {
                      'w': 340,
                      'h': 340,
                      'resize': 'fit'
                    }
                  }
                }
              ]
            },
            'truncated': false,
            'source': '<a href=\'http://twitter.com/download/iphone\' rel=\'nofollow\'>Twitter for iPhone</a>',
            'in_reply_to_status_id': null,
            'in_reply_to_status_id_str': null,
            'in_reply_to_user_id': null,
            'in_reply_to_user_id_str': null,
            'in_reply_to_screen_name': null,
            'user': {
              'id': 15062340,
              'id_str': '15062340',
              'name': '/dev/cloud/jeremy',
              'screen_name': 'jeremycloud',
              'location': 'Madison, Wisconsin',
              'description': 'Professional yak shaver. Amateur bike shedder.',
              'url': 'https://t.co/FcYeBkOpVY',
              'entities': {
                'url': {
                  'urls': [
                    {
                      'url': 'https://t.co/FcYeBkOpVY',
                      'expanded_url': 'http://about.me/jeremy.cloud',
                      'display_url': 'about.me/jeremy.cloud',
                      'indices': [
                        0,
                        23
                      ]
                    }
                  ]
                },
                'description': {
                  'urls': []
                }
              },
              'protected': false,
              'followers_count': 4324,
              'friends_count': 410,
              'listed_count': 103,
              'created_at': 'Mon Jun 09 17:00:58 +0000 2008',
              'favourites_count': 815,
              'utc_offset': -18000,
              'time_zone': 'Central Time (US & Canada)',
              'geo_enabled': true,
              'verified': false,
              'statuses_count': 2218,
              'lang': 'en',
              'contributors_enabled': false,
              'is_translator': false,
              'is_translation_enabled': false,
              'profile_background_color': '000000',
              'profile_background_image_url': 'http://abs.twimg.com/images/themes/theme1/bg.png',
              'profile_background_image_url_https': 'https://abs.twimg.com/images/themes/theme1/bg.png',
              'profile_background_tile': false,
              'profile_image_url': 'http://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
              'profile_image_url_https': 'https://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
              'profile_banner_url': 'https://pbs.twimg.com/profile_banners/15062340/1447451621',
              'profile_link_color': '4A913C',
              'profile_sidebar_border_color': '000000',
              'profile_sidebar_fill_color': '000000',
              'profile_text_color': '000000',
              'profile_use_background_image': false,
              'has_extended_profile': true,
              'default_profile': false,
              'default_profile_image': false,
              'following': true,
              'follow_request_sent': false,
              'notifications': false
            },
            'geo': null,
            'coordinates': null,
            'place': null,
            'contributors': null,
            'is_quote_status': false,
            'retweet_count': 0,
            'favorite_count': 11,
            'favorited': false,
            'retweeted': false,
            'possibly_sensitive': false,
            'possibly_sensitive_appealable': false,
            'lang': 'en'
          },
          'is_quote_status': true,
          'retweet_count': 0,
          'favorite_count': 0,
          'favorited': false,
          'retweeted': false,
          'possibly_sensitive': false,
          'possibly_sensitive_appealable': false,
          'lang': 'en'
        }";

        private static string _fullTextTweetWith1Mention = @"{
          'created_at': 'Thu Mar 10 23:12:12 +0000 2016',
          'id': 708067963060916224,
          'id_str': '708067963060916224',
          'full_text': '@jeremycloud Who would win in a battle between a Barred Owl and a Cooper`s Hawk? https://t.co/FamikDro2h',
          'display_text_range': [
            13,
            80
          ],
          'entities': {
            'hashtags': [],
            'symbols': [],
            'user_mentions': [
              {
                'screen_name': 'jeremycloud',
                'name': '/dev/cloud/jeremy',
                'id': 15062340,
                'id_str': '15062340',
                'indices': [
                  0,
                  12
                ]
              }
            ],
            'urls': [
              {
                'url': 'https://t.co/FamikDro2h',
                'expanded_url': 'https://twitter.com/jeremycloud/status/703621193417379840',
                'display_url': 'twitter.com/jeremycloud/st…',
                'indices': [
                  81,
                  104
                ]
              }
            ]
          },
          'truncated': false,
          'source': 'bonesTwurl',
          'in_reply_to_status_id': 704059336788606976,
          'in_reply_to_status_id_str': '704059336788606976',
          'in_reply_to_user_id': 15062340,
          'in_reply_to_user_id_str': '15062340',
          'in_reply_to_screen_name': 'jeremycloud',
          'user': {
            'id': 4449621923,
            'id_str': '4449621923',
            'name': 'Mr Bones',
            'screen_name': 'MrBonesDroid',
            'location': '',
            'profile_location': null,
            'description': '',
            'url': null,
            'entities': {
            'description': {
	        'urls': []
             }
            },
            'protected': true,
            'followers_count': 5,
            'friends_count': 7,
            'listed_count': 0,
            'created_at': 'Fri Dec 11 15:18:02 +0000 2015',
            'favourites_count': 7,
            'utc_offset': -25200,
            'time_zone': 'Pacific Time (US & Canada)',
            'geo_enabled': false,
            'verified': false,
            'statuses_count': 35,
            'lang': 'en-gb',
            'contributors_enabled': false,
            'is_translator': false,
            'is_translation_enabled': false,
            'profile_background_color': 'F5F8FA',
            'profile_background_image_url': null,
            'profile_background_image_url_https': null,
            'profile_background_tile': false,
            'profile_image_url': 'http://pbs.twimg.com/profile_images/677097663288860672/zZxWCPSI_normal.jpg',
            'profile_image_url_https': 'https://pbs.twimg.com/profile_images/677097663288860672/zZxWCPSI_normal.jpg',
            'profile_link_color': '2B7BB9',
            'profile_sidebar_border_color': 'C0DEED',
            'profile_sidebar_fill_color': 'DDEEF6',
            'profile_text_color': '333333',
            'profile_use_background_image': true,
            'has_extended_profile': false,
            'default_profile': true,
            'default_profile_image': false,
            'following': true,
            'follow_request_sent': false,
            'notifications': false
          },
          'geo': null,
          'coordinates': null,
          'place': null,
          'contributors': null,
          'quoted_status_id': 703621193417379840,
          'quoted_status_id_str': '703621193417379840',
          'quoted_status': {
            'created_at': 'Sat Feb 27 16:42:19 +0000 2016',
            'id': 703621193417379840,
            'id_str': '703621193417379840',
            'full_text': 'Cooper’s Hawk https://t.co/nppuOGne9X',
            'display_text_range': [
              0,
              37
            ],
            'entities': {
              'hashtags': [],
              'symbols': [],
              'user_mentions': [],
              'urls': [],
              'media': [
                {
                  'id': 703621193182502913,
                  'id_str': '703621193182502913',
                  'indices': [
                    14,
                    37
                  ],
                  'media_url': 'http://pbs.twimg.com/media/CcPDv0wUYAE3D-2.jpg',
                  'media_url_https': 'https://pbs.twimg.com/media/CcPDv0wUYAE3D-2.jpg',
                  'url': 'https://t.co/nppuOGne9X',
                  'display_url': 'pic.twitter.com/nppuOGne9X',
                  'expanded_url': 'http://twitter.com/jeremycloud/status/703621193417379840/photo/1',
                  'type': 'photo',
                  'sizes': {
                    'medium': {
                      'w': 600,
                      'h': 398,
                      'resize': 'fit'
                    },
                    'thumb': {
                      'w': 150,
                      'h': 150,
                      'resize': 'crop'
                    },
                    'large': {
                      'w': 1024,
                      'h': 680,
                      'resize': 'fit'
                    },
                    'small': {
                      'w': 340,
                      'h': 226,
                      'resize': 'fit'
                    }
                  }
                }
              ]
            },
            'extended_entities': {
              'media': [
                {
                  'id': 703621193182502913,
                  'id_str': '703621193182502913',
                  'indices': [
                    14,
                    37
                  ],
                  'media_url': 'http://pbs.twimg.com/media/CcPDv0wUYAE3D-2.jpg',
                  'media_url_https': 'https://pbs.twimg.com/media/CcPDv0wUYAE3D-2.jpg',
                  'url': 'https://t.co/nppuOGne9X',
                  'display_url': 'pic.twitter.com/nppuOGne9X',
                  'expanded_url': 'http://twitter.com/jeremycloud/status/703621193417379840/photo/1',
                  'type': 'photo',
                  'sizes': {
                    'medium': {
                      'w': 600,
                      'h': 398,
                      'resize': 'fit'
                    },
                    'thumb': {
                      'w': 150,
                      'h': 150,
                      'resize': 'crop'
                    },
                    'large': {
                      'w': 1024,
                      'h': 680,
                      'resize': 'fit'
                    },
                    'small': {
                      'w': 340,
                      'h': 226,
                      'resize': 'fit'
                    }
                  }
                }
              ]
            },
            'truncated': false,
            'source': '<a href=\'http://www.apple.com/\' rel=\'nofollow\'>OS X</a>',
            'in_reply_to_status_id': null,
            'in_reply_to_status_id_str': null,
            'in_reply_to_user_id': null,
            'in_reply_to_user_id_str': null,
            'in_reply_to_screen_name': null,
            'user': {
              'id': 15062340,
              'id_str': '15062340',
              'name': '/dev/cloud/jeremy',
              'screen_name': 'jeremycloud',
              'location': 'Madison, Wisconsin',
              'description': 'Professional yak shaver. Amateur bike shedder.',
              'url': 'https://t.co/FcYeBkOpVY',
              'entities': {
                'url': {
                  'urls': [
                    {
                      'url': 'https://t.co/FcYeBkOpVY',
                      'expanded_url': 'http://about.me/jeremy.cloud',
                      'display_url': 'about.me/jeremy.cloud',
                      'indices': [
                        0,
                        23
                      ]
                    }
                  ]
                },
                'description': {
                  'urls': []
                }
              },
              'protected': false,
              'followers_count': 4329,
              'friends_count': 411,
              'listed_count': 103,
              'created_at': 'Mon Jun 09 17:00:58 +0000 2008',
              'favourites_count': 803,
              'utc_offset': -21600,
              'time_zone': 'Central Time (US & Canada)',
              'geo_enabled': true,
              'verified': false,
              'statuses_count': 2216,
              'lang': 'en',
              'contributors_enabled': false,
              'is_translator': false,
              'is_translation_enabled': false,
              'profile_background_color': '000000',
              'profile_background_image_url': 'http://abs.twimg.com/images/themes/theme1/bg.png',
              'profile_background_image_url_https': 'https://abs.twimg.com/images/themes/theme1/bg.png',
              'profile_background_tile': false,
              'profile_image_url': 'http://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
              'profile_image_url_https': 'https://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
              'profile_banner_url': 'https://pbs.twimg.com/profile_banners/15062340/1447451621',
              'profile_link_color': '4A913C',
              'profile_sidebar_border_color': '000000',
              'profile_sidebar_fill_color': '000000',
              'profile_text_color': '000000',
              'profile_use_background_image': false,
              'has_extended_profile': true,
              'default_profile': false,
              'default_profile_image': false,
              'following': true,
              'follow_request_sent': false,
              'notifications': false
            },
            'geo': null,
            'coordinates': null,
            'place': null,
            'contributors': null,
            'is_quote_status': false,
            'retweet_count': 0,
            'favorite_count': 2,
            'favorited': false,
            'retweeted': false,
            'possibly_sensitive': false,
            'possibly_sensitive_appealable': false,
            'lang': 'en'
          },
          'is_quote_status': true,
          'retweet_count': 0,
          'favorite_count': 0,
          'favorited': false,
          'retweeted': false,
          'possibly_sensitive': false,
          'possibly_sensitive_appealable': false,
          'lang': 'en'
        }";

        static string _simpleFullTextExtendedTweet = @"{
          'created_at': 'Mon Mar 07 15:13:47 +0000 2016',
          'id': 706860403981099008,
          'id_str': '706860403981099008',
          'full_text': 'Peek-a-boo! https://t.co/R3P6waHxRa',
          'display_text_range': [
            0,
            35
          ],
          'entities': {
            'hashtags': [],
            'symbols': [],
            'user_mentions': [],
            'urls': [],
            'media': [
              {
                'id': 706860403746181121,
                'id_str': '706860403746181121',
                'indices': [
                  12,
                  35
                ],
                'media_url': 'http://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'media_url_https': 'https://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'url': 'https://t.co/R3P6waHxRa',
                'display_url': 'pic.twitter.com/R3P6waHxRa',
                'expanded_url': 'http://twitter.com/jeremycloud/status/706860403981099008/photo/1',
                'type': 'photo',
                'sizes': {
                  'medium': {
                    'w': 600,
                    'h': 398,
                    'resize': 'fit'
                  },
                  'small': {
                    'w': 340,
                    'h': 226,
                    'resize': 'fit'
                  },
                  'thumb': {
                    'w': 150,
                    'h': 150,
                    'resize': 'crop'
                  },
                  'large': {
                    'w': 1024,
                    'h': 680,
                    'resize': 'fit'
                  }
                }
              }
            ]
          },
          'extended_entities': {
            'media': [
              {
                'id': 706860403746181121,
                'id_str': '706860403746181121',
                'indices': [
                  12,
                  35
                ],
                'media_url': 'http://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'media_url_https': 'https://pbs.twimg.com/media/Cc9FyscUkAEQaOw.jpg',
                'url': 'https://t.co/R3P6waHxRa',
                'display_url': 'pic.twitter.com/R3P6waHxRa',
                'expanded_url': 'http://twitter.com/jeremycloud/status/706860403981099008/photo/1',
                'type': 'photo',
                'sizes': {
                  'medium': {
                    'w': 600,
                    'h': 398,
                    'resize': 'fit'
                  },
                  'small': {
                    'w': 340,
                    'h': 226,
                    'resize': 'fit'
                  },
                  'thumb': {
                    'w': 150,
                    'h': 150,
                    'resize': 'crop'
                  },
                  'large': {
                    'w': 1024,
                    'h': 680,
                    'resize': 'fit'
                  }
                }
              }
            ]
          },
          'truncated': false,
          'source': '<a href=\'http://www.apple.com/\' rel=\'nofollow\'>OS X</a>',
          'in_reply_to_status_id': null,
          'in_reply_to_status_id_str': null,
          'in_reply_to_user_id': null,
          'in_reply_to_user_id_str': null,
          'in_reply_to_screen_name': null,
          'user': {
            'id': 15062340,
            'id_str': '15062340',
            'name': '/dev/cloud/jeremy',
            'screen_name': 'jeremycloud',
            'location': 'Madison, Wisconsin',
            'description': 'Professional yak shaver. Amateur bike shedder.',
            'url': 'https://t.co/FcYeBkOpVY',
            'entities': {
              'url': {
                'urls': [
                  {
                    'url': 'https://t.co/FcYeBkOpVY',
                    'expanded_url': 'http://about.me/jeremy.cloud',
                    'display_url': 'about.me/jeremy.cloud',
                    'indices': [
                      0,
                      23
                    ]
                  }
                ]
              },
              'description': {
                'urls': []
              }
            },
            'protected': false,
            'followers_count': 4324,
            'friends_count': 410,
            'listed_count': 103,
            'created_at': 'Mon Jun 09 17:00:58 +0000 2008',
            'favourites_count': 815,
            'utc_offset': -18000,
            'time_zone': 'Central Time (US & Canada)',
            'geo_enabled': true,
            'verified': false,
            'statuses_count': 2218,
            'lang': 'en',
            'contributors_enabled': false,
            'is_translator': false,
            'is_translation_enabled': false,
            'profile_background_color': '000000',
            'profile_background_image_url': 'http://abs.twimg.com/images/themes/theme1/bg.png',
            'profile_background_image_url_https': 'https://abs.twimg.com/images/themes/theme1/bg.png',
            'profile_background_tile': false,
            'profile_image_url': 'http://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
            'profile_image_url_https': 'https://pbs.twimg.com/profile_images/436903139183054849/i_MbCcoW_normal.jpeg',
            'profile_banner_url': 'https://pbs.twimg.com/profile_banners/15062340/1447451621',
            'profile_link_color': '4A913C',
            'profile_sidebar_border_color': '000000',
            'profile_sidebar_fill_color': '000000',
            'profile_text_color': '000000',
            'profile_use_background_image': false,
            'has_extended_profile': true,
            'default_profile': false,
            'default_profile_image': false,
            'following': true,
            'follow_request_sent': false,
            'notifications': false
          },
          'geo': null,
          'coordinates': null,
          'place': null,
          'contributors': null,
          'is_quote_status': false,
          'retweet_count': 0,
          'favorite_count': 8,
          'favorited': false,
          'retweeted': false,
          'possibly_sensitive': false,
          'possibly_sensitive_appealable': false,
          'lang': 'en'
        }";

        private const string SIMPLE_TWEET_THREE_PHOTOS =
                "{\"created_at\":\"Thu May 04 14:45:40 +0000 2017\",\"id\":860143395490189315,\"id_str\":\"860143395490189315\",\"text\":\"hmm https:\\/\\/t.co\\/vzh4H9Lj1u\",\"truncated\":false,\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[],\"urls\":[],\"media\":[{\"id\":860143373545615362,\"id_str\":\"860143373545615362\",\"indices\":[4,27],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_X10wXsAIubBc.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_X10wXsAIubBc.jpg\",\"url\":\"https:\\/\\/t.co\\/vzh4H9Lj1u\",\"display_url\":\"pic.twitter.com\\/vzh4H9Lj1u\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860143395490189315\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"extended_entities\":{\"media\":[{\"id\":860143373545615362,\"id_str\":\"860143373545615362\",\"indices\":[4,27],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_X10wXsAIubBc.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_X10wXsAIubBc.jpg\",\"url\":\"https:\\/\\/t.co\\/vzh4H9Lj1u\",\"display_url\":\"pic.twitter.com\\/vzh4H9Lj1u\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860143395490189315\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860143381992927232,\"id_str\":\"860143381992927232\",\"indices\":[4,27],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_X2UOXcAA-5uq.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_X2UOXcAA-5uq.jpg\",\"url\":\"https:\\/\\/t.co\\/vzh4H9Lj1u\",\"display_url\":\"pic.twitter.com\\/vzh4H9Lj1u\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860143395490189315\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860143391933440000,\"id_str\":\"860143391933440000\",\"indices\":[4,27],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_X25QXoAAC5b7.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_X25QXoAAC5b7.jpg\",\"url\":\"https:\\/\\/t.co\\/vzh4H9Lj1u\",\"display_url\":\"pic.twitter.com\\/vzh4H9Lj1u\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860143395490189315\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/uksocialsense.co.uk\\/\\\" rel=\\\"nofollow\\\"\\u003eSocial Sense UK\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":567,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"und\"}";

        private const string SIMPLE_TWEET_ONE_PHOTO =
                "{\"created_at\":\"Thu May 04 15:28:17 +0000 2017\",\"id\":860154117871849474,\"id_str\":\"860154117871849474\",\"full_text\":\"just one photo https:\\/\\/t.co\\/blFtRxZnUc\",\"truncated\":false,\"display_text_range\":[0,14],\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[],\"urls\":[],\"media\":[{\"id\":860154114646396932,\"id_str\":\"860154114646396932\",\"indices\":[15,38],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_hnCeW0AQYEI6.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_hnCeW0AQYEI6.jpg\",\"url\":\"https:\\/\\/t.co\\/blFtRxZnUc\",\"display_url\":\"pic.twitter.com\\/blFtRxZnUc\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860154117871849474\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"extended_entities\":{\"media\":[{\"id\":860154114646396932,\"id_str\":\"860154114646396932\",\"indices\":[15,38],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_hnCeW0AQYEI6.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_hnCeW0AQYEI6.jpg\",\"url\":\"https:\\/\\/t.co\\/blFtRxZnUc\",\"display_url\":\"pic.twitter.com\\/blFtRxZnUc\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860154117871849474\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/test.commsmanager.visav.co.uk\\\" rel=\\\"nofollow\\\"\\u003eTestCommsManager\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":568,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"en\"}";

        private const string SIMPLE_TWEET_TWO_URLS =
                "{\"created_at\":\"Thu May 04 15:33:30 +0000 2017\",\"id\":860155433234964480,\"id_str\":\"860155433234964480\",\"full_text\":\"test with some URLs https:\\/\\/t.co\\/dWeOSNqwo5 https:\\/\\/t.co\\/KBtBq1lAhx\",\"truncated\":false,\"display_text_range\":[0,67],\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[],\"urls\":[{\"url\":\"https:\\/\\/t.co\\/dWeOSNqwo5\",\"expanded_url\":\"http:\\/\\/google.co.uk\",\"display_url\":\"google.co.uk\",\"indices\":[20,43]},{\"url\":\"https:\\/\\/t.co\\/KBtBq1lAhx\",\"expanded_url\":\"http:\\/\\/bbc.co.uk\",\"display_url\":\"bbc.co.uk\",\"indices\":[44,67]}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/test.commsmanager.visav.co.uk\\\" rel=\\\"nofollow\\\"\\u003eTestCommsManager\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":570,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"en\"}";

        private const string SIMPLE_TWEET_MIXED_ENTITIES =
            "{\"created_at\":\"Thu May 04 15:43:28 +0000 2017\",\"id\":860157941080576000,\"id_str\":\"860157941080576000\",\"full_text\":\"mixed test @VISAV2 @nickvisav https:\\/\\/t.co\\/FpLiZ4h8QK https:\\/\\/t.co\\/KNv4CvJhfl yup https:\\/\\/t.co\\/xckf5pX8gR\",\"truncated\":false,\"display_text_range\":[0,81],\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[{\"screen_name\":\"VISAV2\",\"name\":\"VISAV\",\"id\":1728100111,\"id_str\":\"1728100111\",\"indices\":[11,18]},{\"screen_name\":\"nickvisav\",\"name\":\"Nick Houghton\",\"id\":3384432874,\"id_str\":\"3384432874\",\"indices\":[19,29]}],\"urls\":[{\"url\":\"https:\\/\\/t.co\\/FpLiZ4h8QK\",\"expanded_url\":\"http:\\/\\/mymsg.eu\\/2edx\",\"display_url\":\"mymsg.eu\\/2edx\",\"indices\":[30,53]},{\"url\":\"https:\\/\\/t.co\\/KNv4CvJhfl\",\"expanded_url\":\"http:\\/\\/mymsg.eu\\/2edw\",\"display_url\":\"mymsg.eu\\/2edw\",\"indices\":[54,77]}],\"media\":[{\"id\":860157919907770369,\"id_str\":\"860157919907770369\",\"indices\":[82,105],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_lEiLXcAE8Ejb.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_lEiLXcAE8Ejb.jpg\",\"url\":\"https:\\/\\/t.co\\/xckf5pX8gR\",\"display_url\":\"pic.twitter.com\\/xckf5pX8gR\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860157941080576000\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"extended_entities\":{\"media\":[{\"id\":860157919907770369,\"id_str\":\"860157919907770369\",\"indices\":[82,105],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_lEiLXcAE8Ejb.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_lEiLXcAE8Ejb.jpg\",\"url\":\"https:\\/\\/t.co\\/xckf5pX8gR\",\"display_url\":\"pic.twitter.com\\/xckf5pX8gR\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860157941080576000\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860157928199921664,\"id_str\":\"860157928199921664\",\"indices\":[82,105],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_lFBEXoAAiEMj.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_lFBEXoAAiEMj.jpg\",\"url\":\"https:\\/\\/t.co\\/xckf5pX8gR\",\"display_url\":\"pic.twitter.com\\/xckf5pX8gR\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860157941080576000\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860157937762914304,\"id_str\":\"860157937762914304\",\"indices\":[82,105],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_lFksXUAA4hSe.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_lFksXUAA4hSe.jpg\",\"url\":\"https:\\/\\/t.co\\/xckf5pX8gR\",\"display_url\":\"pic.twitter.com\\/xckf5pX8gR\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860157941080576000\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/uksocialsense.co.uk\\/\\\" rel=\\\"nofollow\\\"\\u003eSocial Sense UK\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":571,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"en\"}";

        private const string EXTENDED_TWEET_THREE_PHOTOS_FETCHED_IN_EXTENDED_MODE = 
            "{\"created_at\":\"Thu May 04 15:50:59 +0000 2017\",\"id\":860159833139531777,\"id_str\":\"860159833139531777\",\"full_text\":\"some long message with images at the end some long message with images at the end some long message with images at the end 12oi3j12o3ij3 end https:\\/\\/t.co\\/tS5h4pyfG8\",\"truncated\":false,\"display_text_range\":[0,140],\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[],\"urls\":[],\"media\":[{\"id\":860159810263711745,\"id_str\":\"860159810263711745\",\"indices\":[141,164],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_mykTWAAE-66t.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_mykTWAAE-66t.jpg\",\"url\":\"https:\\/\\/t.co\\/tS5h4pyfG8\",\"display_url\":\"pic.twitter.com\\/tS5h4pyfG8\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860159833139531777\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"extended_entities\":{\"media\":[{\"id\":860159810263711745,\"id_str\":\"860159810263711745\",\"indices\":[141,164],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_mykTWAAE-66t.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_mykTWAAE-66t.jpg\",\"url\":\"https:\\/\\/t.co\\/tS5h4pyfG8\",\"display_url\":\"pic.twitter.com\\/tS5h4pyfG8\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860159833139531777\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860159821106098176,\"id_str\":\"860159821106098176\",\"indices\":[141,164],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_mzMsXsAAgoKe.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_mzMsXsAAgoKe.jpg\",\"url\":\"https:\\/\\/t.co\\/tS5h4pyfG8\",\"display_url\":\"pic.twitter.com\\/tS5h4pyfG8\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860159833139531777\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}},{\"id\":860159829754732544,\"id_str\":\"860159829754732544\",\"indices\":[141,164],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/C-_mzs6XYAApFug.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/C-_mzs6XYAApFug.jpg\",\"url\":\"https:\\/\\/t.co\\/tS5h4pyfG8\",\"display_url\":\"pic.twitter.com\\/tS5h4pyfG8\",\"expanded_url\":\"https:\\/\\/twitter.com\\/VISAV2\\/status\\/860159833139531777\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"large\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"},\"small\":{\"w\":680,\"h\":510,\"resize\":\"fit\"},\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"medium\":{\"w\":1024,\"h\":768,\"resize\":\"fit\"}}}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/test.commsmanager.visav.co.uk\\\" rel=\\\"nofollow\\\"\\u003eTestCommsManager\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":572,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"en\"}";

        private const string EXTENDED_TWEET_THREE_PHOTOS_FETCHED_IN_COMPAT_MODE =
                "{\"created_at\":\"Thu May 04 15:50:59 +0000 2017\",\"id\":860159833139531777,\"id_str\":\"860159833139531777\",\"text\":\"some long message with images at the end some long message with images at the end some long message with images at\\u2026 https:\\/\\/t.co\\/ImGDtYUGc1\",\"truncated\":true,\"entities\":{\"hashtags\":[],\"symbols\":[],\"user_mentions\":[],\"urls\":[{\"url\":\"https:\\/\\/t.co\\/ImGDtYUGc1\",\"expanded_url\":\"https:\\/\\/twitter.com\\/i\\/web\\/status\\/860159833139531777\",\"display_url\":\"twitter.com\\/i\\/web\\/status\\/8\\u2026\",\"indices\":[116,139]}]},\"source\":\"\\u003ca href=\\\"http:\\/\\/test.commsmanager.visav.co.uk\\\" rel=\\\"nofollow\\\"\\u003eTestCommsManager\\u003c\\/a\\u003e\",\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1728100111,\"id_str\":\"1728100111\",\"name\":\"VISAV\",\"screen_name\":\"VISAV2\",\"location\":\"Nottingham\",\"description\":\"\",\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"entities\":{\"url\":{\"urls\":[{\"url\":\"http:\\/\\/t.co\\/VYML2EZ5Gw\",\"expanded_url\":\"http:\\/\\/www.visav.co.uk\",\"display_url\":\"visav.co.uk\",\"indices\":[0,22]}]},\"description\":{\"urls\":[]}},\"protected\":false,\"followers_count\":90,\"friends_count\":437,\"listed_count\":8,\"created_at\":\"Wed Sep 04 08:38:39 +0000 2013\",\"favourites_count\":14,\"utc_offset\":3600,\"time_zone\":\"London\",\"geo_enabled\":true,\"verified\":false,\"statuses_count\":572,\"lang\":\"en-gb\",\"contributors_enabled\":false,\"is_translator\":false,\"is_translation_enabled\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/378800000408927126\\/a64b39899f9a3a27664029cab14ea7fe_normal.jpeg\",\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"has_extended_profile\":false,\"default_profile\":true,\"default_profile_image\":false,\"following\":false,\"follow_request_sent\":false,\"notifications\":false,\"translator_type\":\"none\"},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"retweet_count\":0,\"favorite_count\":0,\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"possibly_sensitive_appealable\":false,\"lang\":\"en\"}";

        #endregion
    }
}
