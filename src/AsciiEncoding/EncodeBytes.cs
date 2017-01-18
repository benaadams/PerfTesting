using BenchmarkDotNet.Attributes;
using System.Text;

namespace AsciiEncoding
{
    [Config(typeof(CoreConfig))]
    public class EncodeBytes
    {
        private const int InnerLoopCount = 20;

        [Params(
            1,
            16,
            64,
            125,
            250,
            500,
            1000,
            2000,
            4000
        )]
        public int ArrayLength { get; set; }
        public char[] input;
        public byte[] output;

        public Encoding ASCII;
        public Encoding UTF8;
        public Encoding AltASCII;

        [Setup]
        public unsafe void Setup()
        {
            ASCII = Encoding.ASCII;
            UTF8 = Encoding.UTF8;
            AltASCII = AltAsciiEncoding.AltASCII;

            input = new char[ArrayLength];
            output = new byte[ArrayLength];

            for (var i = 0; i < input.Length; i++)
            {
                // ascii chars 32 - 126
                input[i] = (char)((i % (126 - 32)) + 32);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount, Baseline =true)]
        public void ASCIIGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                ASCII.GetBytes(input, 0, input.Length, output, 0);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void UTF8GetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                UTF8.GetBytes(input, 0, input.Length, output, 0);
            }
        }

        [Benchmark(OperationsPerInvoke = InnerLoopCount)]
        public void FastPathGetBytes()
        {
            for (var loop = 0; loop < InnerLoopCount; loop++)
            {
                AltASCII.GetBytes(input, 0, input.Length, output, 0);
            }
        }
    }
}
