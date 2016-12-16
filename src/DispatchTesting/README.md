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
                                  Direct | 1.5664 ns | 0.0322 ns | 638,403,322.00 |
                           DirectWrapper | 1.8262 ns | 0.0152 ns | 547,588,179.69 |
                     DirectSealedWrapper | 1.8141 ns | 0.0301 ns | 551,245,996.24 |
                    DirectVirtualWrapper | 5.3033 ns | 0.0728 ns | 188,560,793.65 |
              DirectVirtualSealedWrapper | 5.2845 ns | 0.0741 ns | 189,231,020.57 |
                       DirectBaseWrapper | 4.7435 ns | 0.0404 ns | 210,814,089.08 |
             DirectBaseNotDerivedWrapper | 4.7326 ns | 0.0587 ns | 211,302,146.00 |
       ViaBaseDerivedWrapperMonomorophic | 5.3173 ns | 0.0602 ns | 188,064,085.39 |
       ViaBaseDerivedWrapperPolymorophic | 5.3248 ns | 0.0554 ns | 187,801,637.14 |
 ViaBaseDerivedSealedWrapperMonomorophic | 5.3438 ns | 0.1423 ns | 187,131,620.68 |
         ManualDerivedWrapperPolymorphic | 5.6642 ns | 0.0610 ns | 176,546,735.81 |
   ManualDerivedSealedWrapperPolymorphic | 5.6572 ns | 0.0514 ns | 176,765,325.06 |
             RuntimeInterfaceMonomorphic | 7.6859 ns | 0.1022 ns | 130,108,647.68 |
             RuntimeInterfacePolymorphic | 8.9126 ns | 0.0982 ns | 112,200,123.57 |
                   DirectInterfaceAsCast | 1.6206 ns | 0.0268 ns | 617,066,786.41 |
              ManualInterfaceMonomorphic | 1.6452 ns | 0.0220 ns | 607,824,298.08 |
              ManualInterfacePolymorphic | 1.6083 ns | 0.0221 ns | 621,781,739.40 |
              InterfaceGenericConstraint | 8.8550 ns | 0.0932 ns | 112,930,510.85 |
```