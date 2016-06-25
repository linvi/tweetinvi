using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Injectinvi
{
    public interface ITweetinviContainer
    {
        bool IsInitialized { get; }
        event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;
        
        void Initialize();

        void RegisterType<T, U>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where U : T;
        void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve);
        void RegisterInstance(Type T, object value);

        T Resolve<T>(params IConstructorNamedParameter[] parameters);
    }
}