using BenchmarkDotNet.Attributes;
using System.Text;

namespace AsciiEncoding
{
    [Config(typeof(CoreConfig))]
    public class EncodeString
    {
        private const int InnerLoopCount = 20;

        [Params(
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            14,
            15,
            16,
            17,
            18,
            32,
            63,
            64,
            65,
            128,
            256,
            512,
            1024
        )]
        public int StringLength { get; set; }
        public string data;

        public Encoding ASCII;
        public Encoding UTF8;
        public Encoding AltASCII;

        [Setup]
        public unsafe void Setup()
        {
            ASCII = Encoding.ASCII;
            AltASCII = AltAsciiEncoding.AltASCII;
            UTF8 = Encoding.UTF8;

            data = new string('\0', StringLength);
            fixed (char* pData = data)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    // ascii chars 32 - 126
                    pData[i] = (char)((i % (126 - 32)) + 32);
                }
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount, Baseline =true)]
        public void ASCIIGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                ASCII.GetBytes(data);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void UTF8GetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                UTF8.GetBytes(data);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void FastPathGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                AltASCII.GetBytes(data);
            }
        }
    }
}
