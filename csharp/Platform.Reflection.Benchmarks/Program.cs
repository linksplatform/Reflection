using BenchmarkDotNet.Running;

namespace Platform.Reflection.Benchmarks
{
    /// <summary>
    /// <para>
    /// Represents the program.
    /// </para>
    /// <para></para>
    /// </summary>
    static class Program
    {
        /// <summary>
        /// <para>
        /// Main.
        /// </para>
        /// <para></para>
        /// </summary>
        static void Main() => BenchmarkRunner.Run<CodeGenerationBenchmarks>();
    }
}
