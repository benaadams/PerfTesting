using System;
using BenchmarkDotNet.Running;

namespace AsciiEncoding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Test();
            BenchmarkRunner.Run<AsciiEncoding>();
        }

        private static unsafe void Test()
        {
            for (var i = 0; i < 512; i++)
            {
                Test(i);
            }
        }

        private static unsafe void Test(int length)
        {
            var data = new string('\0', length);
            fixed (char* pData = data)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    // ascii chars 32 - 126
                    pData[i] = (char)((i % (126 - 32)) + 32);
                }
            }

            var bytes = AltAsciiEncoding.AltASCII.GetBytes(data);

            if (bytes.Length != data.Length)
            {
                throw new InvalidOperationException();
            }

            int c = 0;
            foreach (var ch in data)
            {
                if (ch != bytes[c])
                {
                    throw new InvalidOperationException();
                }
                c++;
            }
        }
    }
}
