using Xunit;

namespace Platform.Reflection.Tests
{
    public class TypeTests
    {
        [Fact]
        public void UInt64IsNumericTest()
        {
            Assert.True(Type<ulong>.IsNumeric);
        }
    }
}
