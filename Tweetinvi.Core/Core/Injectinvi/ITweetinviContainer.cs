using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Injectinvi
{
    public interface ITweetinviContainer
    {
        bool IsInitialized { get; }
        event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;

        void Initialize();

        void RegisterType<TRegistered, TTo>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where TTo : TRegistered;
        void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve);
        void RegisterInstance(Type targetType, object value);

        T ThreadResolve<T>(params IConstructorNamedParameter[] parameters);
        T Resolve<T>(params IConstructorNamedParameter[] parameters);
    }
}