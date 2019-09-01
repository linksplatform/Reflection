using System;
using System.Reflection.Emit;
using Xunit;

namespace Platform.Reflection.Tests
{
    public static class CodeGenerationTests
    {
        [Fact]
        public static void EmptyActionCompilationTest()
        {
            Action compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                generator.Return();
            });
            compiledAction();
        }

        [Fact]
        public static void FailedActionCompilationTest()
        {
            Action compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                throw new NotImplementedException();
            });
            Assert.Throws<NotSupportedException>(compiledAction);
        }
    }
}
