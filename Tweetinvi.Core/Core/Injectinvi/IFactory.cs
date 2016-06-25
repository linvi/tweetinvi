namespace Tweetinvi.Core.Injectinvi
{
    public interface IFactory<T>
    {
        T Create(params IConstructorNamedParameter[] parameters);
        IConstructorNamedParameter GenerateParameterOverrideWrapper(string parameterName, object parameterValue);
    }

    public class TweetinviFactory
    {
        public static IConstructorNamedParameter CreateConstructorParameter(string parameterName, object parameterValue)
        {
            return new ConstructorNamedParameter(parameterName, parameterValue);
        }
    }

    public class Factory<T> : IFactory<T>
    {
        private readonly ITweetinviContainer _container;

        public Factory(ITweetinviContainer container)
        {
            _container = container;
        }

        public T Create(params IConstructorNamedParameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }

        public IConstructorNamedParameter GenerateParameterOverrideWrapper(string parameterName, object parameterValue)
        {
            return new ConstructorNamedParameter(parameterName, parameterValue);
        }
    }
}