## Header Validate testing


```
               Method |                               Value |          Mean |     StdDev |            RPS |
--------------------- |------------------------------------ |-------------- |----------- |--------------- |
   ValidateHeaderIter |                                  13 |     4.7229 ns |  0.0492 ns | 211,733,373.12 |
 ValidateHeaderVector |                                  13 |     5.0420 ns |  0.0366 ns | 198,332,954.88 |
   ValidateHeaderIter |                          text/plain |    16.6734 ns |  0.3520 ns |  59,975,946.30 |
 ValidateHeaderVector |                          text/plain |     6.2288 ns |  0.0638 ns | 160,543,964.49 |
   ValidateHeaderIter |             Content-Security-Policy |    37.6319 ns |  0.2950 ns |  26,573,199.60 |
 ValidateHeaderVector |             Content-Security-Policy |    10.7913 ns |  0.1514 ns |  92,667,513.70 |
   ValidateHeaderIter | Mozilla/5.0 (Windows... 109 byte UA |   115.1069 ns |  3.0876 ns |   8,687,577.53 |
 ValidateHeaderVector | Mozilla/5.0 (Windows... 109 byte UA |    36.4889 ns |  0.3958 ns |  27,405,596.46 |
   ValidateHeaderIter | ZGVmYXVsdC1zcmM... 1168 byte cookie | 1,047.3965 ns | 10.6608 ns |     954,748.25 |
 ValidateHeaderVector | ZGVmYXVsdC1zcmM... 1168 byte cookie |   362.3385 ns |  2.9977 ns |   2,759,850.40 |
```