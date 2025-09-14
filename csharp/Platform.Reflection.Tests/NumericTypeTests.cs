using Xunit;

namespace Platform.Reflection.Tests
{
    public class NumericTypeTests
    {
        [Fact]
        public void UInt64IsNumericTest()
        {
            Assert.True(NumericType<ulong>.IsNumeric);
        }

        [Fact]
        public void NullableIntCanBeNumericTest()
        {
            Assert.True(NumericType<int?>.CanBeNumeric);
        }

        [Fact]
        public void NullableDoubleCanBeNumericTest()
        {
            Assert.True(NumericType<double?>.CanBeNumeric);
        }

        [Fact]
        public void NullableBoolCanBeNumericTest()
        {
            Assert.True(NumericType<bool?>.CanBeNumeric);
        }
    }
}
