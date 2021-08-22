using Xunit;

namespace Platform.Reflection.Tests
{
    /// <summary>
    /// <para>
    /// Represents the numeric type tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class NumericTypeTests
    {
        /// <summary>
        /// <para>
        /// Tests that u int 64 is numeric test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void UInt64IsNumericTest()
        {
            Assert.True(NumericType<ulong>.IsNumeric);
        }
    }
}
