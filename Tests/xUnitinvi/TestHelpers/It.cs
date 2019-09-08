using FakeItEasy;

namespace xUnitinvi.TestHelpers
{
    public static class It
    {
        public static T IsAny<T>()
        {
            return A<T>.Ignored;
        }
    }
}
