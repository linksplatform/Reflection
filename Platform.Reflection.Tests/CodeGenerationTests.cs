using Platform.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Platform.Reflection.Tests
{
    public class CodeGenerationTests
    {
        private readonly ITestOutputHelper _output;

        public CodeGenerationTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void EmptyActionCompilationTest()
        {
            var compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                generator.Return();
            });
            compiledAction();
        }

        [Fact]
        public void FailedActionCompilationTest()
        {
            var compiledAction = DelegateHelpers.Compile<Action>(generator =>
            {
                throw new NotImplementedException();
            });
            Assert.Throws<NotSupportedException>(compiledAction);
        }

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

        private class MethodsContainer
        {
            public static readonly Func<int> DelegateWithoutAggressiveInlining;
            public static readonly Func<int> DelegateWithAggressiveInlining;

            static MethodsContainer()
            {
                void emitCode(System.Reflection.Emit.ILGenerator generator)
                {
                    generator.LoadConstant(72893);
                    generator.LoadConstant(67421);
                    generator.Add();
                    generator.Return();
                };
                DelegateWithoutAggressiveInlining = DelegateHelpers.Compile<Func<int>>(emitCode, aggressiveInlining: false);
                DelegateWithAggressiveInlining = DelegateHelpers.Compile<Func<int>>(emitCode, aggressiveInlining: true);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int WrapperForDelegateWithoutAggressiveInlining() => DelegateWithoutAggressiveInlining();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int WrapperForDelegateWithAggressiveInlining() => DelegateWithAggressiveInlining();
        }

        [Fact]
        public void AggressiveInliningEffectTest()
        {
            const int N = 100000000;
        
            int result = 0;

            var ts4 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.WrapperForDelegateWithAggressiveInlining();
                }
            });

            var ts1 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.DelegateWithoutAggressiveInlining();
                }
            });

            // Currently fastest
            var ts2 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.DelegateWithAggressiveInlining();
                }
            });

            var ts3 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.WrapperForDelegateWithoutAggressiveInlining();
                }
            });

            var output = $"{ts1} {ts2} {ts3} {ts4} {result}";
            _output.WriteLine(output);
        }
    }
}
