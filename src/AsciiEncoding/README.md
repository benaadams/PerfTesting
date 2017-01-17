## ASCII string => byte[] testing


```
           Method | StringLength |          Mean | Scaled |            RPS | Allocated |
----------------- |------------- |-------------- |------- |--------------- |---------- |
    ASCIIGetBytes |            0 |    38.4659 ns |   1.00 |  25,997,064.34 |      55 B |
     UTF8GetBytes |            0 |    34.0826 ns |   0.89 |  29,340,494.77 |      55 B |
 FastPathGetBytes |            0 |     4.3241 ns |   0.11 | 231,260,406.01 |       0 B |
    ASCIIGetBytes |            1 |    35.0843 ns |   1.00 |  28,502,794.72 |      31 B |
     UTF8GetBytes |            1 |    40.7989 ns |   1.16 |  24,510,487.01 |      31 B |
 FastPathGetBytes |            1 |    15.8264 ns |   0.45 |  63,185,503.90 |      31 B |
    ASCIIGetBytes |            2 |    36.7773 ns |   1.00 |  27,190,672.96 |      31 B |
     UTF8GetBytes |            2 |    41.4267 ns |   1.13 |  24,139,002.87 |      31 B |
 FastPathGetBytes |            2 |    17.1762 ns |   0.47 |  58,220,043.08 |      31 B |
    ASCIIGetBytes |            3 |    38.5192 ns |   1.00 |  25,961,074.16 |      31 B |
     UTF8GetBytes |            3 |    44.7293 ns |   1.16 |  22,356,720.37 |      31 B |
 FastPathGetBytes |            3 |    18.0674 ns |   0.47 |  55,348,347.42 |      31 B |
    ASCIIGetBytes |            4 |    40.2293 ns |   1.00 |  24,857,521.12 |      31 B |
     UTF8GetBytes |            4 |    47.8462 ns |   1.19 |  20,900,283.19 |      31 B |
 FastPathGetBytes |            4 |    19.1069 ns |   0.47 |  52,336,994.78 |      31 B |
    ASCIIGetBytes |            5 |    41.9509 ns |   1.00 |  23,837,393.88 |      31 B |
     UTF8GetBytes |            5 |    50.4886 ns |   1.20 |  19,806,446.68 |      31 B |
 FastPathGetBytes |            5 |    19.6285 ns |   0.47 |  50,946,272.29 |      31 B |
    ASCIIGetBytes |            6 |    43.4820 ns |   1.00 |  22,998,030.98 |      31 B |
     UTF8GetBytes |            6 |    52.6119 ns |   1.21 |  19,007,105.66 |      31 B |
 FastPathGetBytes |            6 |    20.4859 ns |   0.47 |  48,814,092.96 |      31 B |
    ASCIIGetBytes |            7 |    45.2872 ns |   1.00 |  22,081,314.12 |      31 B |
     UTF8GetBytes |            7 |    55.0579 ns |   1.22 |  18,162,695.92 |      31 B |
 FastPathGetBytes |            7 |    17.9490 ns |   0.40 |  55,713,395.49 |      31 B |
    ASCIIGetBytes |            8 |    46.9418 ns |   1.00 |  21,302,959.11 |      31 B |
     UTF8GetBytes |            8 |    56.8499 ns |   1.21 |  17,590,186.79 |      31 B |
 FastPathGetBytes |            8 |    18.9719 ns |   0.40 |  52,709,606.78 |      31 B |
    ASCIIGetBytes |           14 |    57.8146 ns |   1.00 |  17,296,670.28 |      39 B |
     UTF8GetBytes |           14 |    74.9049 ns |   1.30 |  13,350,266.65 |      39 B |
 FastPathGetBytes |           14 |    22.5186 ns |   0.39 |  44,407,706.50 |      39 B |
    ASCIIGetBytes |           15 |    59.9135 ns |   1.00 |  16,690,738.98 |      39 B |
     UTF8GetBytes |           15 |    73.0695 ns |   1.22 |  13,685,593.50 |      39 B |
 FastPathGetBytes |           15 |    20.7145 ns |   0.35 |  48,275,309.41 |      39 B |
    ASCIIGetBytes |           16 |    70.1182 ns |   1.00 |  14,261,634.50 |      39 B |
     UTF8GetBytes |           16 |    68.8123 ns |   0.98 |  14,532,275.71 |      39 B |
 FastPathGetBytes |           16 |    21.4978 ns |   0.31 |  46,516,364.33 |      39 B |
    ASCIIGetBytes |           17 |    70.3667 ns |   1.00 |  14,211,261.96 |      47 B |
     UTF8GetBytes |           17 |    73.4145 ns |   1.04 |  13,621,287.74 |      47 B |
 FastPathGetBytes |           17 |    22.9815 ns |   0.33 |  43,513,273.40 |      47 B |
    ASCIIGetBytes |           18 |    74.8212 ns |   1.00 |  13,365,197.96 |      47 B |
     UTF8GetBytes |           18 |    69.5903 ns |   0.93 |  14,369,816.97 |      47 B |
 FastPathGetBytes |           18 |    21.8974 ns |   0.29 |  45,667,563.71 |      47 B |
    ASCIIGetBytes |           32 |   101.3085 ns |   1.00 |   9,870,836.34 |      55 B |
     UTF8GetBytes |           32 |    81.2491 ns |   0.80 |  12,307,823.71 |      55 B |
 FastPathGetBytes |           32 |    27.4950 ns |   0.27 |  36,370,249.41 |      55 B |
    ASCIIGetBytes |           63 |   161.0612 ns |   1.00 |   6,208,819.98 |      87 B |
     UTF8GetBytes |           63 |   116.8431 ns |   0.73 |   8,558,487.07 |      87 B |
 FastPathGetBytes |           63 |    39.1155 ns |   0.24 |  25,565,311.21 |      87 B |
    ASCIIGetBytes |           64 |   163.9895 ns |   1.00 |   6,097,951.76 |      87 B |
     UTF8GetBytes |           64 |   116.5886 ns |   0.71 |   8,577,166.78 |      87 B |
 FastPathGetBytes |           64 |    40.2730 ns |   0.25 |  24,830,551.60 |      87 B |
    ASCIIGetBytes |           65 |   166.8880 ns |   1.00 |   5,992,043.51 |      95 B |
     UTF8GetBytes |           65 |   119.6743 ns |   0.72 |   8,356,013.05 |      95 B |
 FastPathGetBytes |           65 |    40.9087 ns |   0.25 |  24,444,675.47 |      95 B |
    ASCIIGetBytes |          128 |   285.9522 ns |   1.00 |   3,497,088.31 |     151 B |
     UTF8GetBytes |          128 |   209.3091 ns |   0.73 |   4,777,622.91 |     151 B |
 FastPathGetBytes |          128 |    66.6881 ns |   0.23 |  14,995,185.02 |     151 B |
    ASCIIGetBytes |          256 |   531.6415 ns |   1.00 |   1,880,966.89 |     279 B |
     UTF8GetBytes |          256 |   386.5149 ns |   0.73 |   2,587,222.60 |     279 B |
 FastPathGetBytes |          256 |   121.0964 ns |   0.23 |   8,257,883.59 |     279 B |
    ASCIIGetBytes |          512 | 1,025.1415 ns |   1.00 |     975,475.11 |     534 B |
     UTF8GetBytes |          512 |   707.9482 ns |   0.69 |   1,412,532.72 |     534 B |
 FastPathGetBytes |          512 |   240.9107 ns |   0.24 |   4,150,914.96 |     534 B |
    ASCIIGetBytes |         1024 | 2,036.6071 ns |   1.00 |     491,012.73 |   1.04 kB |
     UTF8GetBytes |         1024 | 1,349.8871 ns |   0.66 |     740,802.70 |   1.04 kB |
 FastPathGetBytes |         1024 |   465.0265 ns |   0.23 |   2,150,415.03 |   1.04 kB |
```