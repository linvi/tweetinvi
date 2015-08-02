using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;

namespace Testinvi.Helpers
{
    [ExcludeFromCodeCoverage]
    public class FakeRepository
    {
        private static readonly Dictionary<object, object> _fakedRepository;

        static FakeRepository()
        {
            _fakedRepository = new Dictionary<object, object>();
        }

        public static void ClearRepository()
        {
            _fakedRepository.Clear();
        }

        public static void RegisterFake<T>(Fake<T> fake)
        {
            _fakedRepository.Add(fake.FakedObject, fake);
        }

        public static void RegisterFake(object fake, object fakedObject)
        {
            _fakedRepository.Add(fakedObject, fake);
        }

        public static Fake<T> GetFake<T>(T fakedObject)
        {
            object fake;
            if (_fakedRepository.TryGetValue(fakedObject, out fake))
            {
                if (fake is Fake<T>)
                {
                    return (Fake<T>) fake;
                }
            }

            return null;
        }
    }
}