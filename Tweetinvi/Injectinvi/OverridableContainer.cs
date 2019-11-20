using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Events;

namespace Tweetinvi.Injectinvi
{
    public class OverridableContainer : ITweetinviContainer
    {
        private readonly AutofacContainer _container;

        public OverridableContainer(AutofacContainer container)
        {
            _container = container;
        }

        public bool IsInitialized { get { return false; } }

#pragma warning disable 67
        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;
#pragma warning restore 67

        public void Initialize()
        {
            throw new InvalidOperationException();
        }

        public void RegisterType<TRegistered, TTo>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where TTo : TRegistered
        {
            _container.RegisterType<TRegistered, TTo>(registrationLifetime);

            JsonPropertyConverterRepository.TryOverride<TRegistered, TTo>();
            JsonPropertiesConverterRepository.TryOverride<TRegistered, TTo>();
        }

        public void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve)
        {
            _container.RegisterGeneric(sourceType, targetType, registrationLifetime);
        }

        public void RegisterInstance(Type targetType, object value)
        {
            _container.RegisterInstance(targetType, value);
        }

        public T Resolve<T>(params IConstructorNamedParameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }
    }
}