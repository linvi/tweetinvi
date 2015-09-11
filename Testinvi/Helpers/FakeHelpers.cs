using FakeItEasy;

namespace Testinvi.Helpers
{
    public static class It
    {
        public static T IsAny<T>()
        {
            return A<T>.Ignored;
        }
    }
}