using BenchmarkDotNet.Running;

namespace Platform.Reflection.Benchmarks
{
    static class Program
    {
        static void Main() => BenchmarkRunner.Run<CodeGenerationBenchmarks>();
    }
}
