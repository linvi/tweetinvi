using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Logic;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Wrapper;
using Tweetinvi.Models.DTO;

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
            var tweet = InitTweet(_twitterExample);

            Assert.AreEqual(tweet.Prefix, "@sam @aileen ");
            Assert.AreEqual(tweet.Prefix.Length, 13);
            Assert.AreEqual(tweet.Text, "Check out this photo of @YellowstoneNPS! It makes me want to go camping there this summer. Have you visited before?? nps.gov/yell/index.htm ");
            Assert.AreEqual(tweet.Text.Length, 140);
            Assert.AreEqual(tweet.Suffix, "pic.twitter.com/e8bDiL6LI4");
            Assert.AreEqual(tweet.Suffix.Length, 24);
        }

        [TestMethod]
        public void SimpleFullText()
        {
            var tweet = InitTweet(_simpleFullTextExtendedTweet);

            Assert.AreEqual(tweet.Prefix, "");
            Assert.AreEqual(tweet.Text, "Peek-a-boo! https://t.co/R3P6waHxRa");
            Assert.AreEqual(tweet.Suffix, "");
        }

        [TestMethod]
        public void FullTextWithMentionAndSuffixUrl()
        {
            var tweet = InitTweet(_fullTextTweetWith1Mention);

            Assert.AreEqual(tweet.Prefix, "@jeremycloud ");
            Assert.AreEqual(tweet.Text, "Who would win in a battle between a Barred Owl and a Cooper`s Hawk? ");
            Assert.AreEqual(tweet.Suffix, "https://t.co/FamikDro2h");
        }

        [TestMethod]
        public void ExtendedTweet()
        {
            var tweet = InitTweet(_extendedTweet);

            Assert.AreNotEqual(tweet.TweetDTO.ExtendedTweet, null);
            Assert.AreEqual(tweet.FullText, "@jeremycloud It`s neat to have owls and raccoons around until you realize that raccoons will eat the eggs from the owl`s nest https://t.co/Q0pkaU4ORH");
            Assert.AreEqual(tweet.Entities.UserMentions.Count, 1);
        }

        private static ITweet InitTweet(string text)
        {
            var userFactory = A.Fake<IUserFactory>();
            userFactory.CallsTo(x => x.GenerateUserFromDTO(It.IsAny<IUserDTO>())).Returns(null);

            var jsonConverterLib = new JsonConvertWrapper();
            var jsonConverter = new JsonObjectConverter(jsonConverterLib);

            var tweetDTO = jsonConverter.DeserializeObject<ITweetDTO>(text);
            return new Tweet(tweetDTO, null, null, userFactory, null);
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

        #endregion
    }
}
