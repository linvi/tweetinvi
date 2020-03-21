using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Tweetinvi.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Credentials;
using Tweetinvi.Events;
using Tweetinvi.Logic;
using Tweetinvi.Streams;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Injectinvi
{
    public class TweetinviContainer : ITweetinviContainer
    {
        private IContainer _container;
        private readonly ContainerBuilder _containerBuilder;
        private readonly List<ITweetinviModule> _moduleCatalog;

        public List<Action<ContainerBuilder>> RegistrationActions { get; }

        public ITwitterClient AssociatedClient { get; set; }

        private bool _isInitialized;

        public bool IsInitialized
        {
            get
            {
                lock (_lock)
                {
                    return _isInitialized;
                }
            }
            set
            {
                lock (_lock)
                {
                    _isInitialized = value;
                }
            }
        }

        public TweetinviContainer()
        {
            RegistrationActions = new List<Action<ContainerBuilder>>();

            _containerBuilder = new ContainerBuilder();
            _moduleCatalog = new List<ITweetinviModule>();

            RegisterModules();
            InitializeModules();
        }

        public TweetinviContainer(ITweetinviContainer container)
        {
            RegistrationActions = new List<Action<ContainerBuilder>>();
            _containerBuilder = new ContainerBuilder();

            container.RegistrationActions.ForEach(register =>
            {
                register(_containerBuilder);
                RegistrationActions.Add(register);
            });
        }

        public event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationCompletes;

        private readonly object _lock = new object();

        public void Initialize()
        {
            lock (_lock)
            {
                if (IsInitialized)
                {
                    return;
                }

                this.Raise(BeforeRegistrationCompletes, new TweetinviContainerEventArgs(this));
                BuildContainer();
            }
        }

        public void BuildContainer()
        {
            lock (_lock)
            {
                if (IsInitialized)
                {
                    return;
                }

                _container = _containerBuilder.Build();
                IsInitialized = true;
            }
        }

        private void RegisterModules()
        {
            _moduleCatalog.Add(new TweetinviModule());
            _moduleCatalog.Add(new TweetinviControllersModule());
            _moduleCatalog.Add(new TweetinviCoreModule(this));
            _moduleCatalog.Add(new TweetinviCredentialsModule());
            _moduleCatalog.Add(new TweetinviLogicModule());
            _moduleCatalog.Add(new TweetinviWebLogicModule());

            _moduleCatalog.Add(new StreaminviModule());
        }

        private void InitializeModules()
        {
            foreach (var module in _moduleCatalog)
            {
                module.Initialize(this);
            }
        }

        public virtual void RegisterType<TRegistered, TTo>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where TTo : TRegistered
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Cannot update container after it was already initialized");
            }

            Action<ContainerBuilder> registrationAction;

            switch (registrationLifetime)
            {
                case RegistrationLifetime.InstancePerResolve:
                    registrationAction = builder => builder.RegisterType<TTo>().As<TRegistered>();
                    break;
                case RegistrationLifetime.InstancePerApplication:
                    registrationAction = builder => builder.RegisterType<TTo>().As<TRegistered>().SingleInstance();
                    break;
                default:
                    throw new NotSupportedException("This operation is not supported");
            }

            JsonPropertyConverterRepository.TryOverride<TRegistered, TTo>();
            JsonPropertiesConverterRepository.TryOverride<TRegistered, TTo>();

            registrationAction(_containerBuilder);
            RegistrationActions.Add(registrationAction);
        }

        public virtual void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Cannot update container after it was already initialized");
            }


            Action<ContainerBuilder> registrationAction;

            switch (registrationLifetime)
            {
                case RegistrationLifetime.InstancePerResolve:
                    registrationAction = builder => builder.RegisterGeneric(targetType).As(sourceType);
                    break;
                case RegistrationLifetime.InstancePerApplication:
                    registrationAction = builder => builder.RegisterGeneric(targetType).As(sourceType).SingleInstance();
                    break;
                default:
                    throw new NotSupportedException("This operation is not supported");
            }

            registrationAction(_containerBuilder);
            RegistrationActions.Add(registrationAction);
        }

        public virtual void RegisterInstance(Type targetType, object value)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Cannot update container after it was already initialized");
            }

            var registrationAction = new Action<ContainerBuilder>(builder => { builder.RegisterInstance(value).As(targetType).ExternallyOwned(); });

            registrationAction(_containerBuilder);
            RegistrationActions.Add(registrationAction);
        }

        public void RegisterDecorator<TDecorator, TDecorated>() where TDecorator : TDecorated
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException("Cannot update container after it was already initialized");
            }

            var registrationAction = new Action<ContainerBuilder>(builder =>
            {
                builder.RegisterDecorator<TDecorator, TDecorated>();
            });

            registrationAction(_containerBuilder);
            RegistrationActions.Add(registrationAction);
        }

        public T Resolve<T>(params IConstructorNamedParameter[] parameters)
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("The container has not yet been built!");
            }

            return _container.Resolve<T>(parameters.Select(p => new NamedParameter(p.Name, p.Value)));
        }
    }
}