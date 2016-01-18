using System;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Logic.JsonConverters;

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
        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;

        public void Initialize()
        {
            throw new InvalidOperationException();
        }

        public void RegisterType<T, U>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where U : T
        {
            _container.RegisterType<T, U>(registrationLifetime);

            JsonPropertyConverterRepository.TryOverride<T, U>();
            JsonPropertiesConverterRepository.TryOverride<T, U>();
        }

        public void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve)
        {
            _container.RegisterGeneric(sourceType, targetType, registrationLifetime);
        }

        public void RegisterInstance(Type T, object value)
        {
            _container.RegisterInstance(T, value);
        }

        public T Resolve<T>(params IConstructorNamedParameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }
    }
}