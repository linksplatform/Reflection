using System;
using System.Reflection;
using Xunit;
using Platform.Collections;
using Platform.Collections.Lists;

namespace Platform.Reflection.Tests
{
    /// <summary>
    /// <para>
    /// Represents the get il bytes method tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public static class GetILBytesMethodTests
    {
        /// <summary>
        /// <para>
        /// Tests that il bytes for delegate are available test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public static void ILBytesForDelegateAreAvailableTest()
        {
            var function = new Func<object, int>(argument => 0);
            var bytes = function.GetMethodInfo().GetILBytes();
            Assert.False(bytes.IsNullOrEmpty());
        }

        /// <summary>
        /// <para>
        /// Tests that il bytes for different delegates are the same test.
        /// </para>
        /// <para></para>
        /// </summary>
        [Fact]
        public static void ILBytesForDifferentDelegatesAreTheSameTest()
        {
            var firstFunction = new Func<object, int>(argument => 0);
            var secondFunction = new Func<object, int>(argument => 0);
            Assert.False(firstFunction == secondFunction);
            var firstFunctionBytes = firstFunction.GetMethodInfo().GetILBytes();
            Assert.False(firstFunctionBytes.IsNullOrEmpty());
            var secondFunctionBytes = secondFunction.GetMethodInfo().GetILBytes();
            Assert.False(secondFunctionBytes.IsNullOrEmpty());
            Assert.True(firstFunctionBytes.EqualTo(secondFunctionBytes));
        }
    }
}
