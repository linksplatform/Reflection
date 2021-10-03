using System;
using Xunit;

namespace Platform.Reflection.Tests
{
    /// <summary>
    /// <para>
    /// Represents the code generation tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class CodeGenerationTests
    {
        /// <summary>
        /// <para>
        /// Tests that empty action compilation test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void EmptyActionCompilationTest()
        {
            var compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                generator.Return();
            });
            compiledAction();
        }

        /// <summary>
        /// <para>
        /// Tests that failed action compilation test.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// <para></para>
        /// <para></para>
        /// </exception>
        [Fact]
        public void FailedActionCompilationTest()
        {
            var compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                throw new NotImplementedException();
            });
            Assert.Throws<NotSupportedException>(compiledAction);
        }

        /// <summary>
        /// <para>
        /// Tests that constant loading test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void ConstantLoadingTest()
        {
            CheckConstantLoading<byte>(8);
            CheckConstantLoading<uint>(8);
            CheckConstantLoading<ushort>(8);
            CheckConstantLoading<ulong>(8);
        }

        private void CheckConstantLoading<T>(T value)
        {
            var compiledFunction = DelegateHelpers.Compile<Func<T>>(generator =>
            {
                generator.LoadConstant(value);
                generator.Return();
            });
            Assert.Equal(value, compiledFunction());
        }

        /// <summary>
        /// <para>
        /// Tests that unsigned integers conversion with sign extension test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void UnsignedIntegersConversionWithSignExtensionTest()
        {
            object[] withSignExtension = new object[]
            {
                CompileUncheckedConverter<byte, sbyte>(extendSign: true)(128),
                CompileUncheckedConverter<byte, short>(extendSign: true)(128),
                CompileUncheckedConverter<ushort, short>(extendSign: true)(32768),
                CompileUncheckedConverter<byte, int>(extendSign: true)(128),
                CompileUncheckedConverter<ushort, int>(extendSign: true)(32768),
                CompileUncheckedConverter<uint, int>(extendSign: true)(2147483648),
                CompileUncheckedConverter<byte, long>(extendSign: true)(128),
                CompileUncheckedConverter<ushort, long>(extendSign: true)(32768),
                CompileUncheckedConverter<uint, long>(extendSign: true)(2147483648),
                CompileUncheckedConverter<ulong, long>(extendSign: true)(9223372036854775808)
            };
            object[] withoutSignExtension = new object[]
            {
                CompileUncheckedConverter<byte, sbyte>(extendSign: false)(128),
                CompileUncheckedConverter<byte, short>(extendSign: false)(128),
                CompileUncheckedConverter<ushort, short>(extendSign: false)(32768),
                CompileUncheckedConverter<byte, int>(extendSign: false)(128),
                CompileUncheckedConverter<ushort, int>(extendSign: false)(32768),
                CompileUncheckedConverter<uint, int>(extendSign: false)(2147483648),
                CompileUncheckedConverter<byte, long>(extendSign: false)(128),
                CompileUncheckedConverter<ushort, long>(extendSign: false)(32768),
                CompileUncheckedConverter<uint, long>(extendSign: false)(2147483648),
                CompileUncheckedConverter<ulong, long>(extendSign: false)(9223372036854775808)
            };
            var i = 0;
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.NotEqual(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
        }

        /// <summary>
        /// <para>
        /// Tests that signed integers conversion of minus one with sign extension test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SignedIntegersConversionOfMinusOneWithSignExtensionTest()
        {
            object[] withSignExtension = new object[]
            {
                CompileUncheckedConverter<sbyte, byte>(extendSign: true)(-1),
                CompileUncheckedConverter<sbyte, ushort>(extendSign: true)(-1),
                CompileUncheckedConverter<short, ushort>(extendSign: true)(-1),
                CompileUncheckedConverter<sbyte, uint>(extendSign: true)(-1),
                CompileUncheckedConverter<short, uint>(extendSign: true)(-1),
                CompileUncheckedConverter<int, uint>(extendSign: true)(-1),
                CompileUncheckedConverter<sbyte, ulong>(extendSign: true)(-1),
                CompileUncheckedConverter<short, ulong>(extendSign: true)(-1),
                CompileUncheckedConverter<int, ulong>(extendSign: true)(-1),
                CompileUncheckedConverter<long, ulong>(extendSign: true)(-1)
            };
            object[] withoutSignExtension = new object[]
            {
                CompileUncheckedConverter<sbyte, byte>(extendSign: false)(-1),
                CompileUncheckedConverter<sbyte, ushort>(extendSign: false)(-1),
                CompileUncheckedConverter<short, ushort>(extendSign: false)(-1),
                CompileUncheckedConverter<sbyte, uint>(extendSign: false)(-1),
                CompileUncheckedConverter<short, uint>(extendSign: false)(-1),
                CompileUncheckedConverter<int, uint>(extendSign: false)(-1),
                CompileUncheckedConverter<sbyte, ulong>(extendSign: false)(-1),
                CompileUncheckedConverter<short, ulong>(extendSign: false)(-1),
                CompileUncheckedConverter<int, ulong>(extendSign: false)(-1),
                CompileUncheckedConverter<long, ulong>(extendSign: false)(-1)
            };
            var i = 0;
            Assert.Equal((byte)255, (byte)withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal((ushort)65535, (ushort)withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal((ushort)65535, (ushort)withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(4294967295, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(4294967295, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(4294967295, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(18446744073709551615, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(18446744073709551615, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(18446744073709551615, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
            Assert.Equal(18446744073709551615, withSignExtension[i]);
            Assert.Equal(withSignExtension[i], withoutSignExtension[i++]);
        }

        /// <summary>
        /// <para>
        /// Tests that signed integers conversion of two with sign extension test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public void SignedIntegersConversionOfTwoWithSignExtensionTest()
        {
            object[] withSignExtension = new object[]
            {
                CompileUncheckedConverter<sbyte, byte>(extendSign: true)(2),
                CompileUncheckedConverter<sbyte, ushort>(extendSign: true)(2),
                CompileUncheckedConverter<short, ushort>(extendSign: true)(2),
                CompileUncheckedConverter<sbyte, uint>(extendSign: true)(2),
                CompileUncheckedConverter<short, uint>(extendSign: true)(2),
                CompileUncheckedConverter<int, uint>(extendSign: true)(2),
                CompileUncheckedConverter<sbyte, ulong>(extendSign: true)(2),
                CompileUncheckedConverter<short, ulong>(extendSign: true)(2),
                CompileUncheckedConverter<int, ulong>(extendSign: true)(2),
                CompileUncheckedConverter<long, ulong>(extendSign: true)(2)
            };
            object[] withoutSignExtension = new object[]
            {
                CompileUncheckedConverter<sbyte, byte>(extendSign: false)(2),
                CompileUncheckedConverter<sbyte, ushort>(extendSign: false)(2),
                CompileUncheckedConverter<short, ushort>(extendSign: false)(2),
                CompileUncheckedConverter<sbyte, uint>(extendSign: false)(2),
                CompileUncheckedConverter<short, uint>(extendSign: false)(2),
                CompileUncheckedConverter<int, uint>(extendSign: false)(2),
                CompileUncheckedConverter<sbyte, ulong>(extendSign: false)(2),
                CompileUncheckedConverter<short, ulong>(extendSign: false)(2),
                CompileUncheckedConverter<int, ulong>(extendSign: false)(2),
                CompileUncheckedConverter<long, ulong>(extendSign: false)(2)
            };
            for (var i = 0; i < withSignExtension.Length; i++)
            {
                Assert.Equal(2UL, Convert.ToUInt64(withSignExtension[i]));
                Assert.Equal(withSignExtension[i], withoutSignExtension[i]);
            }
        }

        private static Converter<TSource, TTarget> CompileUncheckedConverter<TSource, TTarget>(bool extendSign)
        {
            return DelegateHelpers.Compile<Converter<TSource, TTarget>>(generator =>
            {
                generator.LoadArgument(0);
                generator.UncheckedConvert<TSource, TTarget>(extendSign);
                generator.Return();
            });
        }
    }
}
