## TypeCache Dispatch Testing
```
                      Method |          Mean |     StdDev | Scaled |          RPS |
---------------------------- |-------------- |----------- |------- |------------- |
        InterfaceMonomorphic | 1,276.6333 ns | 16.6153 ns |   1.00 |   783,310.26 |
      InterfacePolymorphicX2 | 1,444.7386 ns | 25.2325 ns |   1.13 |   692,166.75 |
     InterfaceMegamorphicX33 | 1,428.9762 ns | 13.8861 ns |   1.12 |   699,801.72 |

InterfaceUnsafeCastConstTest |   830.3120 ns |  9.8983 ns |   0.65 | 1,204,366.58 |
  InterfaceUnsafeCastVarTest | 1,198.1936 ns | 16.6743 ns |   0.94 |   834,589.69 |

     DirectViaCastIndirected | 1,136.0911 ns | 19.6327 ns |   0.89 |   880,211.12 |
     DirectViaCastNotInlined |   679.2567 ns |  9.4285 ns |   0.53 | 1,472,197.44 |
        DirectViaCastInlined |   192.5118 ns |  5.2530 ns |   0.15 | 5,194,485.75 |
```