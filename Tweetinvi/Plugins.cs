using System;
using System.Reflection;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public static class Plugins
    {
        public static void Add<T>() where T : ITweetinviModule
        {
            var type = typeof(T);

            var ctor = type.GetConstructor(new Type[0]);
            var instance = ctor.Invoke(null);
            var module = (T)instance;

            TweetinviContainer.BeforeRegistrationComplete += (sender, args) =>
            {
                module.Initialize(args.TweetinviContainer);
            };
        }
    }
}
