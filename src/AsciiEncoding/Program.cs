using System;
using BenchmarkDotNet.Running;
using System.Text;

namespace AsciiEncoding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Test();
            // BenchmarkRunner.Run<EncodeString>();
            BenchmarkRunner.Run<EncodeBytes>();
        }

        private static unsafe void Test()
        {
            TestNonAscii();

            for (var i = 0; i < 512; i++)
            {
                Test(i);
            }
        }


        private static void TestNonAscii()
        {
            var test = "FooBA\u0400R";
            var altUtf8 = AltUtf8Encoding.AltUtf8.GetBytes(test);
            var utf8 = Encoding.UTF8.GetBytes(test);

            if (altUtf8.Length != utf8.Length)
            {
                throw new InvalidOperationException();
            }

            for(var i = 0; i < altUtf8.Length; i++)
            {
                if (altUtf8[i] != utf8[i])
                {
                    throw new InvalidOperationException();
                }
            }


            var altASCII = AltAsciiEncoding.AltASCII.GetBytes(test);
            var ascii = Encoding.ASCII.GetBytes(test);

            if (altASCII.Length != ascii.Length)
            {
                throw new InvalidOperationException();
            }

            for (var i = 0; i < altASCII.Length; i++)
            {
                if (altASCII[i] != ascii[i])
                {
                    throw new InvalidOperationException();
                }
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
