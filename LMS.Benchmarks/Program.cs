using BenchmarkDotNet.Running;
using LMS.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        // This switches your console app into benchmark mode
        BenchmarkRunner.Run<BookFetchBenchmarks>();
    }
}