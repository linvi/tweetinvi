using System.Collections.Generic;
using System.Globalization;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Models
{
    public interface IUserDictionary<T>
    {
        T this[long userId] { get; }
        T this[string username] { get; }
        T this[IUserIdentifier index] { get; }
    }

    public class UserDictionary<T> : IUserDictionary<T>
    {
        private readonly Dictionary<string, T> _userIdDictionary;
        private readonly Dictionary<string, T> _usernameDictionary;
        private readonly Dictionary<IUserIdentifier, T> _userIdentifierDictionary;

        public UserDictionary()
        {
            _userIdDictionary = new Dictionary<string, T>();
            _usernameDictionary = new Dictionary<string, T>();
            _userIdentifierDictionary = new Dictionary<IUserIdentifier, T>();
        }

        public T this[long userId] => _userIdDictionary[userId.ToString(CultureInfo.InvariantCulture)];

        public T this[string username] => _usernameDictionary[username];

        public T this[IUserIdentifier index] => _userIdentifierDictionary[index];

        public void AddOrUpdate(IUserIdentifier user, T element)
        {
            _userIdentifierDictionary.AddOrUpdate(user, element);

            if (user.Id > 0)
            {
                _userIdDictionary.AddOrUpdate(user.Id.ToString(CultureInfo.InvariantCulture), element);
            }

            if (!string.IsNullOrEmpty(user.IdStr))
            {
                _userIdDictionary.AddOrUpdate(user.IdStr, element);
            }

            if (user.ScreenName != null)
            {
                _usernameDictionary.AddOrUpdate(user.ScreenName, element);
            }
        }
    }
}
