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
                                  Method |      Mean |    StdDev |    Median |        RPS |
---------------------------------------- |---------- |---------- |---------- |----------- |
                                  Direct | 1.4265 us | 0.0162 us | 1.4338 us | 701,034.11 |
                           DirectWrapper | 1.7331 us | 0.0172 us | 1.7366 us | 577,011.56 |
                     DirectSealedWrapper | 1.7944 us | 0.0367 us | 1.7966 us | 557,290.11 |
                    DirectVirtualWrapper | 2.4158 us | 0.0388 us | 2.4176 us | 413,947.69 |
              DirectVirtualSealedWrapper | 2.4108 us | 0.0280 us | 2.4210 us | 414,808.29 |
                       DirectBaseWrapper | 2.2651 us | 0.0223 us | 2.2691 us | 441,481.21 |
             DirectBaseNotDerivedWrapper | 2.2590 us | 0.0263 us | 2.2693 us | 442,667.75 |
       ViaBaseDerivedWrapperMonomorophic | 2.4090 us | 0.0280 us | 2.4139 us | 415,115.85 |
       ViaBaseDerivedWrapperPolymorophic | 2.4429 us | 0.0747 us | 2.4185 us | 409,353.02 |
 ViaBaseDerivedSealedWrapperMonomorophic | 2.4018 us | 0.0289 us | 2.4115 us | 416,360.34 |
         ManualDerivedWrapperPolymorphic | 3.9783 us | 0.0461 us | 3.9922 us | 251,365.18 |
   ManualDerivedSealedWrapperPolymorphic | 2.9305 us | 0.0318 us | 2.9447 us | 341,237.66 |
             RuntimeInterfaceMonomorphic | 2.3390 us | 0.0185 us | 2.3422 us | 427,536.68 |
            RuntimeInterfacePolymorphic2 | 2.9914 us | 0.0387 us | 3.0056 us | 334,286.07 |
            RuntimeInterfacePolymorphic3 | 3.4293 us | 0.0419 us | 3.4234 us | 291,606.87 |
            RuntimeInterfacePolymorphic4 | 3.4146 us | 0.0775 us | 3.4117 us | 292,855.89 |
            RuntimeInterfacePolymorphic5 | 3.3518 us | 0.0435 us | 3.3550 us | 298,345.58 |
            RuntimeInterfacePolymorphic6 | 3.4023 us | 0.0453 us | 3.4029 us | 293,920.12 |
            RuntimeInterfacePolymorphic7 | 3.3041 us | 0.0370 us | 3.3170 us | 302,658.70 |
            RuntimeInterfacePolymorphic8 | 3.2976 us | 0.0481 us | 3.3183 us | 303,247.94 |
            RuntimeInterfacePolymorphic9 | 3.3232 us | 0.0312 us | 3.3323 us | 300,911.71 |
           RuntimeInterfacePolymorphic10 | 3.2999 us | 0.0412 us | 3.3145 us | 303,042.67 |
           RuntimeInterfacePolymorphic11 | 3.3112 us | 0.0271 us | 3.3180 us | 302,006.02 |
           RuntimeInterfacePolymorphic12 | 3.3047 us | 0.0372 us | 3.3088 us | 302,599.42 |
           RuntimeInterfacePolymorphic13 | 3.3117 us | 0.0348 us | 3.3240 us | 301,959.25 |
           RuntimeInterfacePolymorphic14 | 3.3176 us | 0.0419 us | 3.3228 us | 301,422.55 |
           RuntimeInterfacePolymorphic15 | 3.2763 us | 0.0439 us | 3.2815 us | 305,222.82 |
           RuntimeInterfacePolymorphic16 | 3.3065 us | 0.0363 us | 3.3162 us | 302,436.71 |
           RuntimeInterfacePolymorphic17 | 3.2951 us | 0.0441 us | 3.3071 us | 303,479.10 |
           RuntimeInterfacePolymorphic18 | 3.2851 us | 0.0423 us | 3.3040 us | 304,400.90 |
           RuntimeInterfacePolymorphic19 | 3.3170 us | 0.0415 us | 3.3248 us | 301,480.53 |
           RuntimeInterfacePolymorphic20 | 3.3061 us | 0.0411 us | 3.3194 us | 302,471.92 |
           RuntimeInterfacePolymorphic21 | 3.3059 us | 0.0431 us | 3.3116 us | 302,492.29 |
           RuntimeInterfacePolymorphic22 | 3.3018 us | 0.0499 us | 3.3096 us | 302,862.89 |
           RuntimeInterfacePolymorphic23 | 3.2823 us | 0.0453 us | 3.3036 us | 304,666.91 |
           RuntimeInterfacePolymorphic24 | 3.3088 us | 0.0439 us | 3.3176 us | 302,225.22 |
           RuntimeInterfacePolymorphic25 | 3.3025 us | 0.0400 us | 3.3098 us | 302,801.26 |
           RuntimeInterfacePolymorphic26 | 3.2988 us | 0.0446 us | 3.3145 us | 303,144.39 |
           RuntimeInterfacePolymorphic27 | 3.2866 us | 0.0396 us | 3.3036 us | 304,261.68 |
           RuntimeInterfacePolymorphic28 | 3.3675 us | 0.1091 us | 3.3371 us | 296,956.61 |
           RuntimeInterfacePolymorphic29 | 3.3015 us | 0.0387 us | 3.3101 us | 302,889.10 |
           RuntimeInterfacePolymorphic30 | 3.3266 us | 0.0858 us | 3.3074 us | 300,610.69 |
           RuntimeInterfacePolymorphic31 | 3.3032 us | 0.0439 us | 3.3153 us | 302,740.43 |
           RuntimeInterfacePolymorphic32 | 3.3545 us | 0.0845 us | 3.3309 us | 298,106.33 |
           RuntimeInterfacePolymorphic33 | 3.3156 us | 0.0345 us | 3.3266 us | 301,606.56 |
           RuntimeInterfacePolymorphic34 | 3.3085 us | 0.0361 us | 3.3130 us | 302,255.06 |
           RuntimeInterfacePolymorphic35 | 3.2898 us | 0.0431 us | 3.3049 us | 303,969.54 |
           RuntimeInterfacePolymorphic36 | 3.3192 us | 0.0290 us | 3.3260 us | 301,276.19 |
                   DirectInterfaceAsCast | 3.0819 us | 0.0269 us | 3.0862 us | 324,475.42 |
              ManualInterfaceMonomorphic | 2.7119 us | 0.0229 us | 2.7176 us | 368,741.44 |
              ManualInterfacePolymorphic | 3.8909 us | 0.0497 us | 3.9133 us | 257,008.64 |
              InterfaceGenericConstraint | 1.9589 us | 0.0192 us | 1.9623 us | 510,503.17 |
```