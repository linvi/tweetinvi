using System;
using System.Linq;
using Autofac;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Injectinvi
{
    public class AutofacThreadContainer : ITweetinviContainer
    {
#pragma warning disable 67
        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;
#pragma warning restore 67
        private readonly ILifetimeScope _container;

        public AutofacThreadContainer(IContainer container)
        {
            _container = container.BeginLifetimeScope();
        }

        public bool IsInitialized
        {
            get { return _container != null; }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void RegisterType<T, U>(RegistrationLifetime registrationLifetime) where U : T
        {
            throw new NotImplementedException();
        }

        public void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance(Type T, object value)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(params IConstructorNamedParameter[] parameters)
        {
            return _container.Resolve<T>(parameters.Select(p => new NamedParameter(p.Name, p.Value)));
        }
    }
}
