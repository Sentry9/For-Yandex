using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Homework.Optimization.Core;

[MemoryDiagnoser(false)]
public class SearchAlgorithmsBenchmarks
{
    [Params(10, 1000, 1_000_000, 1_000_000_000)]
    public int ArraySize { get; set; }

    public int[] SortedArray { get; private set; }

    public int Left { get; private set; }
    public int Middle { get; private set; }
    public int Right { get; private set; }

    [GlobalSetup]
    public void Setup()
    {
        SortedArray = new int[ArraySize];

        for (int i = 0; i < ArraySize; i++)
        {
            SortedArray[i] = i + 1;
        }

        Left = 1;
        Middle = ArraySize / 2;
        Right = ArraySize;
    }

    [Benchmark]
    public void FindElementInSortedArrayBenchmark()
    {
        SearchAlgorithms.FindElementInSortedArray(SortedArray, Left);
        SearchAlgorithms.FindElementInSortedArray(SortedArray, Middle);
        SearchAlgorithms.FindElementInSortedArray(SortedArray, Right);
    }

    [Benchmark]
    public void FindElementInSortedArrayOptimizedBenchmark()
    {
        SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Left);
        SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Middle);
        SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Right);
    }
}