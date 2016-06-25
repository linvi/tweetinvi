namespace Tweetinvi.Core.Injectinvi
{
    public interface IConstructorNamedParameter
    {
        string Name { get; }
        object Value { get; }
    }

    public class ConstructorNamedParameter : IConstructorNamedParameter
    {
        public ConstructorNamedParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }
    }
}