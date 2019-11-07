using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace Platform.Reflection.Benchmarks
{
    [SimpleJob]
    [MemoryDiagnoser]
    public class CodeGenerationBenchmarks
    {
        private class MethodsContainer
        {
            public static readonly Func<int> TypeMemberMethodDelegate = DelegateHelpers.Compile<Func<int>>(EmitCode, typeMemberMethod: false);
            public static readonly Func<int> DynamicMethodDelegate = DelegateHelpers.Compile<Func<int>>(EmitCode, typeMemberMethod: true);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int TypeMemberMethodDelegateWrapper() => TypeMemberMethodDelegate();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int DynamicMethodDelegateWrapper() => DynamicMethodDelegate();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void EmitCode(System.Reflection.Emit.ILGenerator generator)
            {
                generator.LoadConstant(140314);
                generator.Return();
            }
        }

        private static int Function() => 140314;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int NotInlinedFunction() => 140314;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlinedFunction() => 140314;

        [Benchmark]
        public int TypeMemberMethodDelegate() => MethodsContainer.TypeMemberMethodDelegate();

        [Benchmark]
        public int DynamicMethodDelegate() => MethodsContainer.DynamicMethodDelegate();

        [Benchmark]
        public int TypeMemberMethodDelegateWrapper() => MethodsContainer.TypeMemberMethodDelegateWrapper();

        [Benchmark]
        public int DynamicMethodDelegateWrapper() => MethodsContainer.DynamicMethodDelegateWrapper();

        [Benchmark]
        public int StaticPrivateFunctionWithAggressiveInlining() => InlinedFunction();

        [Benchmark]
        public int StaticPrivateFunctionWithNoInlining() => NotInlinedFunction();

        [Benchmark]
        public int StaticPrivateFunction() => Function();

        [Benchmark]
        public int BareConstant() => 140314;
    }
}
