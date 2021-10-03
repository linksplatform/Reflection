using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

#pragma warning disable CA1822 // Mark members as static

namespace Platform.Reflection.Benchmarks
{
    /// <summary>
    /// <para>
    /// Represents the code generation benchmarks.
    /// </para>
    /// <para></para>
    /// </summary>
    [SimpleJob]
    [MemoryDiagnoser]
    public class CodeGenerationBenchmarks
    {
        private class MethodsContainer
        {
            /// <summary>
            /// <para>
            /// The type member method.
            /// </para>
            /// <para></para>
            /// </summary>
            public static readonly Func<int> TypeMemberMethodDelegate = DelegateHelpers.Compile<Func<int>>(EmitCode, typeMemberMethod: false);
            /// <summary>
            /// <para>
            /// The type member method.
            /// </para>
            /// <para></para>
            /// </summary>
            public static readonly Func<int> DynamicMethodDelegate = DelegateHelpers.Compile<Func<int>>(EmitCode, typeMemberMethod: true);

            /// <summary>
            /// <para>
            /// Types the member method delegate wrapper.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <returns>
            /// <para>The int</para>
            /// <para></para>
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int TypeMemberMethodDelegateWrapper() => TypeMemberMethodDelegate();

            /// <summary>
            /// <para>
            /// Dynamics the method delegate wrapper.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <returns>
            /// <para>The int</para>
            /// <para></para>
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int DynamicMethodDelegateWrapper() => DynamicMethodDelegate();

            /// <summary>
            /// <para>
            /// Emits the code using the specified generator.
            /// </para>
            /// <para></para>
            /// </summary>
            /// <param name="generator">
            /// <para>The generator.</para>
            /// <para></para>
            /// </param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void EmitCode(System.Reflection.Emit.ILGenerator generator)
            {
                generator.LoadConstant(140314);
                generator.Return();
            }
        }

        private static int Function() => 140314;

        /// <summary>
        /// <para>
        /// Nots the inlined function.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int NotInlinedFunction() => 140314;

        /// <summary>
        /// <para>
        /// Inlineds the function.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlinedFunction() => 140314;

        /// <summary>
        /// <para>
        /// Types the member method delegate.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int TypeMemberMethodDelegate() => MethodsContainer.TypeMemberMethodDelegate();

        /// <summary>
        /// <para>
        /// Dynamics the method delegate.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int DynamicMethodDelegate() => MethodsContainer.DynamicMethodDelegate();

        /// <summary>
        /// <para>
        /// Types the member method delegate wrapper.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int TypeMemberMethodDelegateWrapper() => MethodsContainer.TypeMemberMethodDelegateWrapper();

        /// <summary>
        /// <para>
        /// Dynamics the method delegate wrapper.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int DynamicMethodDelegateWrapper() => MethodsContainer.DynamicMethodDelegateWrapper();

        private function with aggressive inlining.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int StaticPrivateFunctionWithAggressiveInlining() => InlinedFunction();

        private function with no inlining.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int StaticPrivateFunctionWithNoInlining() => NotInlinedFunction();

        private function.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int StaticPrivateFunction() => Function();

        /// <summary>
        /// <para>
        /// Bares the constant.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <returns>
        /// <para>The int</para>
        /// <para></para>
        /// </returns>
        [Benchmark]
        public int BareConstant() => 140314;
    }
}
