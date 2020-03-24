using System;
using System.Collections.Generic;
using Autofac;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Injectinvi
{
    public interface ITweetinviContainer
    {
        bool IsInitialized { get; }
        List<Action<ContainerBuilder>> RegistrationActions { get; }
        ITwitterClient AssociatedClient { get; set; }
        event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;

        void Initialize();

        void RegisterType<TRegistered, TTo>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where TTo : TRegistered;
        void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve);
        void RegisterInstance(Type targetType, object value);
        void RegisterDecorator<TDecorator, TDecorated>() where TDecorator : TDecorated;
        T Resolve<T>(params IConstructorNamedParameter[] parameters);
    }
}