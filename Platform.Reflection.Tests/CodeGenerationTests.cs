using System;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;
using Platform.Diagnostics;

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
                    generator.LoadConstant(140314);
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
            const int N = 10000000;
        
            int result = 0;

            // Warm up

            for (int i = 0; i < N; i++)
            {
                result = MethodsContainer.DelegateWithoutAggressiveInlining();
            }
            for (int i = 0; i < N; i++)
            {
                result = MethodsContainer.DelegateWithAggressiveInlining();
            }
            for (int i = 0; i < N; i++)
            {
                result = MethodsContainer.WrapperForDelegateWithoutAggressiveInlining();
            }
            for (int i = 0; i < N; i++)
            {
                result = MethodsContainer.WrapperForDelegateWithAggressiveInlining();
            }
            for (int i = 0; i < N; i++)
            {
                result = Function();
            }
            for (int i = 0; i < N; i++)
            {
                result = 140314;
            }

            // Measure
            var ts1 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.DelegateWithoutAggressiveInlining();
                }
            });
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
            var ts4 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = MethodsContainer.WrapperForDelegateWithAggressiveInlining();
                }
            });
            var ts5 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = Function();
                }
            });
            var ts6 = Performance.Measure(() =>
            {
                for (int i = 0; i < N; i++)
                {
                    result = 140314;
                }
            });

            var output = $"{ts1} {ts2} {ts3} {ts4} {ts5} {ts6} {result}";
            _output.WriteLine(output);

            Assert.True(ts5 < ts1);
            Assert.True(ts5 < ts2);
            Assert.True(ts5 < ts3);
            Assert.True(ts5 < ts4);
            Assert.True(ts6 < ts1);
            Assert.True(ts6 < ts2);
            Assert.True(ts6 < ts3);
            Assert.True(ts6 < ts4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Function() => 140314;
    }
}
