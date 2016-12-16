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
                                  Method |      Mean |    StdDev |        RPS |
---------------------------------------- |---------- |---------- |----------- |
                                  Direct | 1.6485 us | 0.0229 us | 606,599.34 |
                           DirectWrapper | 1.7307 us | 0.0203 us | 577,790.01 |
                     DirectSealedWrapper | 1.7425 us | 0.0311 us | 573,875.97 |
                    DirectVirtualWrapper | 2.4562 us | 0.0355 us | 407,138.18 |
              DirectVirtualSealedWrapper | 2.4568 us | 0.0373 us | 407,039.02 |
                       DirectBaseWrapper | 2.2462 us | 0.0337 us | 445,202.11 |
             DirectBaseNotDerivedWrapper | 2.2765 us | 0.0383 us | 439,262.17 |
       ViaBaseDerivedWrapperMonomorophic | 2.4770 us | 0.0283 us | 403,708.69 |
       ViaBaseDerivedWrapperPolymorophic | 2.4836 us | 0.0252 us | 402,644.77 |
 ViaBaseDerivedSealedWrapperMonomorophic | 2.5154 us | 0.0859 us | 397,557.70 |
         ManualDerivedWrapperPolymorphic | 4.4621 us | 0.0135 us | 224,108.95 |
   ManualDerivedSealedWrapperPolymorphic | 3.2495 us | 0.0131 us | 307,742.08 |
             RuntimeInterfaceMonomorphic | 2.5816 us | 0.0895 us | 387,353.86 |
             RuntimeInterfacePolymorphic | 3.4119 us | 0.1262 us | 293,091.53 |
                   DirectInterfaceAsCast | 3.3396 us | 0.0483 us | 299,437.47 |
              ManualInterfaceMonomorphic | 2.5741 us | 0.0428 us | 388,483.63 |
              ManualInterfacePolymorphic | 4.0875 us | 0.0778 us | 244,645.76 |
              InterfaceGenericConstraint | 1.8896 us | 0.0137 us | 529,201.40 |
```