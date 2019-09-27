using System;
using System.Collections.Generic;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Models
{
    public interface IUserDictionary<T>
    {
        T this[long? userId] { get; }
        T this[string username] { get; }
        T this[IUserIdentifier index] { get; }
    }

    public class UserDictionary<T> : IUserDictionary<T>
    {
        private readonly Dictionary<long?, T> _userIdDictionary;
        private readonly Dictionary<string, T> _usernameDictionary;
        private readonly Dictionary<IUserIdentifier, T> _userIdentifierDictionary;

        public UserDictionary()
        {
            _userIdDictionary = new Dictionary<long?, T>();
            _usernameDictionary = new Dictionary<string, T>();
            _userIdentifierDictionary = new Dictionary<IUserIdentifier, T>();
        }

        public T this[long? userId]
        {
            get
            {
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

                return _userIdDictionary[userId];
            }
        }

        public T this[string username]
        {
            get
            {
                if (username == null)
                {
                    throw new ArgumentNullException(nameof(username));
                }

                return _usernameDictionary[username];
            }
        }

        public T this[IUserIdentifier index]
        {
            get { return _userIdentifierDictionary[index]; }
        }

        public void AddOrUpdate(IUserIdentifier user, T element)
        {
            _userIdentifierDictionary.AddOrUpdate(user, element);

            if (user.Id != null && user.Id != TweetinviSettings.DEFAULT_ID)
            {
                _userIdDictionary.AddOrUpdate(user.Id, element);
            }

            if (user.ScreenName != null)
            {
                _usernameDictionary.AddOrUpdate(user.ScreenName, element);
            }
        }
    }
}
