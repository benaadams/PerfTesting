## Dispatch testing

// * Summary *

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=4
Frequency=2825622 Hz, Resolution=353.9044 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-003131
  [Host]     : .NET Core 4.6.24628.01, 64bit RyuJIT
  Job-WEXLRO : .NET Core 4.6.24628.01, 64bit RyuJIT

RemoveOutliers=False  Runtime=Core  Server=True
LaunchCount=3  RunStrategy=Throughput  TargetCount=10
WarmupCount=5  Allocated=0 B

```
                                  Method |      Mean |    StdDev |            RPS |
---------------------------------------- |---------- |---------- |--------------- |
                                  Direct | 1.5865 ns | 0.0278 ns | 630,333,676.93 |
                           DirectWrapper | 1.7602 ns | 0.0263 ns | 568,131,618.49 |
                     DirectSealedWrapper | 1.7689 ns | 0.0235 ns | 565,332,916.73 |
                    DirectVirtualWrapper | 5.3065 ns | 0.0662 ns | 188,447,682.85 |
              DirectVirtualSealedWrapper | 1.7585 ns | 0.0174 ns | 568,676,516.75 |
                       DirectBaseWrapper | 4.7525 ns | 0.0344 ns | 210,413,688.88 |
             DirectBaseNotDerivedWrapper | 4.7278 ns | 0.0578 ns | 211,516,259.12 |
       ViaBaseDerivedWrapperMonomorophic | 5.3437 ns | 0.0356 ns | 187,137,105.36 |
       ViaBaseDerivedWrapperPolymorophic | 5.3084 ns | 0.0661 ns | 188,382,134.55 |
 ViaBaseDerivedSealedWrapperMonomorophic | 5.3218 ns | 0.0467 ns | 187,905,052.57 |
         ManualDerivedWrapperPolymorphic | 5.6845 ns | 0.0212 ns | 175,917,728.36 |
             RuntimeInterfaceMonomorphic | 7.6621 ns | 0.1076 ns | 130,512,988.50 |
             RuntimeInterfacePolymorphic | 8.9368 ns | 0.0873 ns | 111,897,064.83 |
                   DirectInterfaceAsCast | 1.6148 ns | 0.0132 ns | 619,254,028.98 |
              ManualInterfaceMonomorphic | 1.6200 ns | 0.0203 ns | 617,265,092.92 |
              ManualInterfacePolymorphic | 1.5892 ns | 0.0194 ns | 629,237,342.28 |
```