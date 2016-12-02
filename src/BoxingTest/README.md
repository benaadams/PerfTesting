

## Integer Box caching

// * Summary *

BenchmarkDotNet=v0.10.0
OS=Windows
Processor=?, ProcessorCount=4
Frequency=2825623 Hz, Resolution=353.9043 ns, Timer=TSC
Host Runtime=.NET Core 4.0.0.0, Arch=64-bit  [RyuJIT]
GC=Concurrent Server
dotnet cli version=1.0.0-preview2-1-003155
Job Runtime(s):
        .NET Core 4.0.0.0, Arch=64-bit  [RyuJIT]

RemoveOutliers=False  Runtime=Core  Server=True
LaunchCount=3  RunStrategy=Throughput  TargetCount=10
WarmupCount=5

```
                   Method |      Mean |    StdDev |    Median | Scaled | Scaled-StdDev |            RPS |
------------------------- |---------- |---------- |---------- |------- |-------------- |--------------- |
      Int32UncachedBoxing | 7.4069 ns | 0.0498 ns | 7.3884 ns |   1.00 |          0.00 | 135,008,742.93 |
        Int32CachedBoxing | 5.3065 ns | 0.0495 ns | 5.3282 ns |   0.72 |          0.01 | 188,448,857.58 |
 Int32CachedBoxExtenstion | 6.7465 ns | 0.0880 ns | 6.7784 ns |   0.91 |          0.01 | 148,226,092.97 |
```

## Boolean Box caching

// * Summary *

BenchmarkDotNet=v0.10.0
OS=Windows
Processor=?, ProcessorCount=4
Frequency=2825623 Hz, Resolution=353.9043 ns, Timer=TSC
Host Runtime=.NET Core 4.0.0.0, Arch=64-bit  [RyuJIT]
GC=Concurrent Server
dotnet cli version=1.0.0-preview2-1-003155
Job Runtime(s):
        .NET Core 4.0.0.0, Arch=64-bit  [RyuJIT]

RemoveOutliers=False  Runtime=Core  Server=True
LaunchCount=3  RunStrategy=Throughput  TargetCount=10
WarmupCount=5

```
                  Method |      Mean |    StdDev |    Median | Scaled | Scaled-StdDev |            RPS |
------------------------ |---------- |---------- |---------- |------- |-------------- |--------------- |
      BoolUncachedBoxing | 7.3923 ns | 0.0391 ns | 7.3866 ns |   1.00 |          0.00 | 135,276,250.40 |
        BoolCachedBoxing | 4.5859 ns | 0.0310 ns | 4.5954 ns |   0.62 |          0.01 | 218,057,656.08 |
 BoolCachedBoxExtenstion | 4.5874 ns | 0.0428 ns | 4.5988 ns |   0.62 |          0.01 | 217,986,777.33 |
 ```