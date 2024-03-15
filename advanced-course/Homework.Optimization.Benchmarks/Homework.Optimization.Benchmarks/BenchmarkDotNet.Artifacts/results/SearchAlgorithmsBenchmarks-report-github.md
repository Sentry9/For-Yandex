```

BenchmarkDotNet v0.13.7, Windows 10 (10.0.19045.3803/22H2/2022Update)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2


```
|                                     Method |  ArraySize |               Mean |              Error |             StdDev | Allocated |
|------------------------------------------- |----------- |-------------------:|-------------------:|-------------------:|----------:|
|          **FindElementInSortedArrayBenchmark** |         **10** |           **8.821 ns** |          **0.1989 ns** |          **0.2443 ns** |         **-** |
| FindElementInSortedArrayOptimizedBenchmark |         10 |          12.152 ns |          0.2735 ns |          0.2686 ns |         - |
|          **FindElementInSortedArrayBenchmark** |       **1000** |         **525.492 ns** |         **10.5445 ns** |         **15.7825 ns** |         **-** |
| FindElementInSortedArrayOptimizedBenchmark |       1000 |          29.757 ns |          0.5979 ns |          1.1082 ns |         - |
|          **FindElementInSortedArrayBenchmark** |    **1000000** |     **539,694.870 ns** |     **10,641.4642 ns** |     **15,927.6425 ns** |       **1 B** |
| FindElementInSortedArrayOptimizedBenchmark |    1000000 |          58.578 ns |          1.1565 ns |          1.0252 ns |         - |
|          **FindElementInSortedArrayBenchmark** | **1000000000** | **600,490,475.000 ns** | **11,909,982.8845 ns** | **31,166,349.3654 ns** |     **600 B** |
| FindElementInSortedArrayOptimizedBenchmark | 1000000000 |         104.855 ns |          2.1228 ns |          2.3594 ns |         - |
